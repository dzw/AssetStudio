using K4os.Compression.LZ4;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using ZstdSharp;

namespace AssetStudio
{
    /// <summary>
    /// A reader for bundle file blocks with caching support.
    /// Caches decompressed blocks to avoid redundant decompression when reading multiple entries.
    /// </summary>
    internal class BundleFileBlockReader : IDisposable
    {
        private const int MaxMemoryStreamLength = 50 * 1024 * 1024; // 50 MB
        private const int MaxPreAllocatedMemoryStreamLength = 30 * 1024 * 1024; // 30 MB

        private readonly Stream m_stream;
        private readonly BundleFile.StorageBlock[] m_blocks;
        private readonly long m_dataOffset;

        private MemoryStream m_cachedBlockStream;
        private int m_cachedBlockIndex = -1;
        private bool m_isDisposed;

        public BundleFileBlockReader(Stream stream, BundleFile.StorageBlock[] blocks, long dataOffset)
        {
            m_stream = stream ?? throw new ArgumentNullException(nameof(stream));
            m_blocks = blocks ?? throw new ArgumentNullException(nameof(blocks));
            m_dataOffset = dataOffset;
        }

        /// <summary>
        /// Read an entry from the bundle.
        /// Uses caching to avoid redundant block decompression.
        /// </summary>
        public Stream ReadEntry(BundleFile.Node entry)
        {
            if (m_isDisposed)
            {
                throw new ObjectDisposedException(nameof(BundleFileBlockReader));
            }

            // Optimization: If there's only one uncompressed block, return a partial stream directly
            if (m_blocks.Length == 1 && GetCompressionType(m_blocks[0]) == CompressionType.None)
            {
                if (m_dataOffset + entry.offset + entry.size > m_stream.Length)
                {
                    throw new IOException("Entry extends beyond the end of the stream.");
                }
                return CreatePartialStream(m_dataOffset + entry.offset, entry.size);
            }

            // Find block offsets for the entry
            int blockIndex;
            long blockCompressedOffset = 0;
            long blockDecompressedOffset = 0;

            for (blockIndex = 0; blockDecompressedOffset + m_blocks[blockIndex].uncompressedSize <= entry.offset; blockIndex++)
            {
                blockCompressedOffset += m_blocks[blockIndex].compressedSize;
                blockDecompressedOffset += m_blocks[blockIndex].uncompressedSize;
            }

            long entryOffsetInsideBlock = entry.offset - blockDecompressedOffset;
            var entryStream = CreateStream(entry.size);
            long left = entry.size;
            m_stream.Position = m_dataOffset + blockCompressedOffset;

            // Copy data from all blocks used by current entry
            while (left > 0)
            {
                byte[] rentedArray = null;
                long blockStreamOffset;
                Stream blockStream;
                var block = m_blocks[blockIndex];

                if (m_cachedBlockIndex == blockIndex)
                {
                    // Data of the previous entry is in the same block as this one
                    // Use cached stream instead of decompressing again
                    blockStreamOffset = 0;
                    blockStream = m_cachedBlockStream;
                    m_stream.Position += block.compressedSize;
                }
                else
                {
                    var compressType = GetCompressionType(block);

                    if (compressType == CompressionType.None)
                    {
                        blockStreamOffset = m_dataOffset + blockCompressedOffset;
                        blockStream = m_stream;
                    }
                    else
                    {
                        blockStreamOffset = 0;
                        m_cachedBlockIndex = blockIndex;

                        if (m_cachedBlockStream != null)
                        {
                            m_cachedBlockStream.Dispose();
                        }
                        m_cachedBlockStream = CreateTemporaryStream(block.uncompressedSize, out rentedArray);

                        DecompressBlock(m_stream, block, m_cachedBlockStream, compressType);
                        blockStream = m_cachedBlockStream;
                    }
                }

                long blockSize = block.uncompressedSize - entryOffsetInsideBlock;
                blockStream.Position = blockStreamOffset + entryOffsetInsideBlock;
                entryOffsetInsideBlock = 0;

                long size = Math.Min(blockSize, left);
                if (blockStream.Position + size > blockStream.Length)
                {
                    throw new IOException("Block extends beyond the end of the stream.");
                }

                CopyStream(blockStream, entryStream, size);
                blockIndex++;

                blockCompressedOffset += block.compressedSize;
                left -= size;

                if (rentedArray != null)
                {
                    ArrayPool<byte>.Shared.Return(rentedArray);
                }
            }

            entryStream.Position = 0;
            return entryStream;
        }

        private CompressionType GetCompressionType(BundleFile.StorageBlock block)
        {
            return (CompressionType)(block.flags & StorageBlockFlags.CompressionTypeMask);
        }

        private void DecompressBlock(Stream source, BundleFile.StorageBlock block, Stream destination, CompressionType compressionType)
        {
            switch (compressionType)
            {
                case CompressionType.Lzma:
                    SevenZipHelper.StreamDecompress(source, destination, block.compressedSize, block.uncompressedSize);
                    break;

                case CompressionType.Lz4:
                case CompressionType.Lz4HC:
                    var compressedBytes = ArrayPool<byte>.Shared.Rent((int)block.compressedSize);
                    try
                    {
                        source.Read(compressedBytes, 0, (int)block.compressedSize);
                        var uncompressedBytes = ArrayPool<byte>.Shared.Rent((int)block.uncompressedSize);
                        try
                        {
                            int bytesWritten = LZ4Codec.Decode(compressedBytes, 0, (int)block.compressedSize,
                                uncompressedBytes, 0, (int)block.uncompressedSize);
                            if (bytesWritten != block.uncompressedSize)
                            {
                                throw new IOException($"LZ4 decompression error: expected {block.uncompressedSize} bytes, got {bytesWritten}");
                            }
                            destination.Write(uncompressedBytes, 0, (int)block.uncompressedSize);
                        }
                        finally
                        {
                            ArrayPool<byte>.Shared.Return(uncompressedBytes);
                        }
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(compressedBytes);
                    }
                    break;

                case CompressionType.Lzham:
                    throw new NotSupportedException("Lzham compression is not supported.");

                default:
                    // Try Zstd detection
                    long pos = source.Position;
                    byte[] signature = new byte[4];
                    source.Read(signature, 0, 4);
                    source.Position = pos;

                    if (signature[0] == 0x28 && signature[1] == 0xB5 && signature[2] == 0x2F && signature[3] == 0xFD)
                    {
                        var compressedData = new byte[block.compressedSize];
                        source.Read(compressedData, 0, (int)block.compressedSize);
                        using (var decompressor = new Decompressor())
                        {
                            var decompressed = decompressor.Unwrap(compressedData);
                            destination.Write(decompressed.ToArray(), 0, decompressed.Length);
                        }
                    }
                    else
                    {
                        throw new NotSupportedException($"Unsupported compression type: {compressionType}");
                    }
                    break;
            }
        }

        private Stream CreateStream(long size)
        {
            if (size > MaxMemoryStreamLength)
            {
                string tempFile = Path.GetTempFileName();
                return new FileStream(tempFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.DeleteOnClose);
            }
            else if (size > MaxPreAllocatedMemoryStreamLength)
            {
                return new MemoryStream();
            }
            else
            {
                return new MemoryStream(new byte[size]);
            }
        }

        private MemoryStream CreateTemporaryStream(long size, out byte[] rentedArray)
        {
            if (size > MaxMemoryStreamLength)
            {
                rentedArray = null;
                // For caching, we still use memory for smaller blocks
                return new MemoryStream(new byte[size]);
            }
            else
            {
                rentedArray = ArrayPool<byte>.Shared.Rent((int)size);
                return new MemoryStream(rentedArray, 0, (int)size, true);
            }
        }

        private Stream CreatePartialStream(long offset, long size)
        {
            m_stream.Position = offset;
            var data = new byte[size];
            m_stream.Read(data, 0, (int)size);
            return new MemoryStream(data);
        }

        private void CopyStream(Stream source, Stream destination, long count)
        {
            byte[] buffer = new byte[81920];
            long remaining = count;
            while (remaining > 0)
            {
                int toRead = (int)Math.Min(buffer.Length, remaining);
                int read = source.Read(buffer, 0, toRead);
                if (read == 0) break;
                destination.Write(buffer, 0, read);
                remaining -= read;
            }
        }

        public void Dispose()
        {
            if (!m_isDisposed)
            {
                m_cachedBlockStream?.Dispose();
                m_cachedBlockStream = null;
                m_isDisposed = true;
            }
        }
    }
}
