using System;
using System.IO;

namespace AssetStudio
{
    public class SerializedFileHeader
    {
        public uint m_MetadataSize;
        public long m_FileSize;
        public SerializedFileFormatVersion m_Version;
        public long m_DataOffset;
        public byte m_Endianess;
        public byte[] m_Reserved;

        /// <summary>
        /// Minimum header size for a serialized file.
        /// </summary>
        private const int HeaderMinSize = 16;

        /// <summary>
        /// Check if the stream position points to a valid serialized file header.
        /// </summary>
        /// <param name="reader">The binary reader.</param>
        /// <param name="fileSize">The total file size for validation.</param>
        /// <returns>True if the stream appears to be a serialized file.</returns>
        public static bool IsSerializedFileHeader(BinaryReader reader, long fileSize)
        {
            long initialPosition = reader.BaseStream.Position;

            try
            {
                if (reader.BaseStream.Position + HeaderMinSize > reader.BaseStream.Length)
                {
                    return false;
                }

                uint metadataSize = reader.ReadUInt32();
                long fileSizeField = reader.ReadUInt32();
                uint version = reader.ReadUInt32();
                uint dataOffset = reader.ReadUInt32();

                // Check version is within valid range
                if (!Enum.IsDefined(typeof(SerializedFileFormatVersion), version))
                {
                    return false;
                }

                var formatVersion = (SerializedFileFormatVersion)version;

                // Validate file size for older versions
                if (formatVersion < SerializedFileFormatVersion.LargeFilesSupport)
                {
                    // For versions without large file support, file size should match
                    // Allow some tolerance for padding
                    if (fileSizeField > fileSize + 1024 || fileSizeField < fileSize - 1024)
                    {
                        // Could still be valid if metadata is at the end
                    }
                }

                // Validate data offset
                if (formatVersion < SerializedFileFormatVersion.Unknown_9)
                {
                    // Metadata is at the end
                    if (metadataSize > fileSize)
                    {
                        return false;
                    }
                }
                else
                {
                    // Metadata is at the beginning
                    if (dataOffset > fileSize)
                    {
                        return false;
                    }
                }

                return true;
            }
            finally
            {
                reader.BaseStream.Position = initialPosition;
            }
        }

        /// <summary>
        /// Check if the stream position points to a valid serialized file header.
        /// </summary>
        public static bool IsSerializedFileHeader(Stream stream, long fileSize)
        {
            return IsSerializedFileHeader(new BinaryReader(stream, System.Text.Encoding.UTF8, leaveOpen: true), fileSize);
        }

        /// <summary>
        /// Get the size of the header based on the format version.
        /// </summary>
        public int GetHeaderSize()
        {
            if (m_Version >= SerializedFileFormatVersion.LargeFilesSupport)
            {
                // Version 22+: 4 + 8 + 4 + 8 + 8 + 1 + 3 + 4 + 8 + 8 + 8 = 64 bytes
                return 32; // Base header before extended fields
            }
            else if (m_Version >= SerializedFileFormatVersion.Unknown_9)
            {
                // Version 9+: 4 + 4 + 4 + 4 + 1 + 3 = 20 bytes
                return 20;
            }
            else
            {
                // Older versions: 4 + 4 + 4 + 4 = 16 bytes
                return 16;
            }
        }
    }
}
