using System;
using System.IO;

namespace AssetStudio.Streams
{
    /// <summary>
    /// A smart stream with reference counting and automatic memory management.
    /// Streams larger than 50MB are automatically stored in temporary files.
    /// </summary>
    public sealed partial class SmartStream : Stream
    {
        /// <summary>
        /// The arbitrary maximum size of a decompressed stream to be stored in RAM. 50 MB
        /// </summary>
        public const int MaxMemoryStreamLength = 50 * 1024 * 1024;

        /// <summary>
        /// The arbitrary maximum size of a decompressed stream to be pre-allocated. 30 MB
        /// </summary>
        public const int MaxPreAllocatedMemoryStreamLength = 30 * 1024 * 1024;

        private SmartStream()
        {
            RefCounter = new SmartRefCount();
        }

        private SmartStream(Stream baseStream)
        {
            Stream = baseStream ?? throw new ArgumentNullException(nameof(baseStream));
            RefCounter = new SmartRefCount();
            RefCounter.Increase();
        }

        private SmartStream(SmartStream copy)
        {
            Assign(copy);
        }

        /// <summary>
        /// Create a SmartStream from a file path.
        /// </summary>
        public static SmartStream OpenRead(string path)
        {
            return new SmartStream(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read));
        }

        /// <summary>
        /// Create a temporary file stream for large data.
        /// </summary>
        public static SmartStream CreateTemp()
        {
            string tempFile = Path.GetTempFileName();
            return new SmartStream(new FileStream(tempFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.DeleteOnClose));
        }

        /// <summary>
        /// Create an in-memory stream.
        /// </summary>
        public static SmartStream CreateMemory()
        {
            return new SmartStream(new MemoryStream());
        }

        /// <summary>
        /// Create an in-memory stream with a pre-allocated buffer.
        /// </summary>
        public static SmartStream CreateMemory(byte[] buffer)
        {
            return new SmartStream(new MemoryStream(buffer));
        }

        /// <summary>
        /// Create an in-memory stream with a slice of a buffer.
        /// </summary>
        public static SmartStream CreateMemory(byte[] buffer, int offset, int size, bool writable = true)
        {
            return new SmartStream(new MemoryStream(buffer, offset, size, writable));
        }

        /// <summary>
        /// Create a SmartStream with appropriate storage based on size.
        /// Uses memory for small data, temporary file for large data.
        /// </summary>
        public static SmartStream CreateStream(long size)
        {
            if (size > MaxMemoryStreamLength)
            {
                return CreateTemp();
            }
            else if (size > MaxPreAllocatedMemoryStreamLength)
            {
                return CreateMemory();
            }
            else
            {
                return CreateMemory(new byte[size]);
            }
        }

        /// <summary>
        /// Create a SmartStream with no backing stream.
        /// </summary>
        public static SmartStream CreateNull()
        {
            return new SmartStream();
        }

        /// <summary>
        /// Copy the reference from another SmartStream.
        /// </summary>
        public void Assign(SmartStream source)
        {
            FreeReference();

            Stream = source.Stream;
            RefCounter = source.RefCounter;
            if (!IsNull)
            {
                RefCounter.Increase();
            }
        }

        /// <summary>
        /// Move the reference from another SmartStream to this.
        /// </summary>
        public void Move(SmartStream source)
        {
            Assign(source);
            source.FreeReference();
        }

        /// <summary>
        /// Create a new reference to the backing stream.
        /// </summary>
        public SmartStream CreateReference()
        {
            return new SmartStream(this);
        }

        /// <summary>
        /// Create a partial stream view.
        /// </summary>
        public SmartStream CreatePartial(long offset, long size)
        {
            ThrowIfNull();

            if (Stream is FileStream fileStream)
            {
                var partialStream = new RandomAccessStream(fileStream, offset, size);
                var result = new SmartStream(this);
                result.Stream = partialStream;
                return result;
            }

            // Copy data for other stream types
            byte[] buffer = new byte[(int)size];
            long initialPosition = Stream.Position;
            Stream.Position = offset;
            Stream.Read(buffer, 0, (int)size);
            Stream.Position = initialPosition;
            return CreateMemory(buffer);
        }

        public override void Flush()
        {
            if (Stream != null)
            {
                Stream.Flush();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            ThrowIfNull();
            return Stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            ThrowIfNull();
            return Stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            ThrowIfNull();
            Stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            ThrowIfNull();
            Stream.Write(buffer, offset, count);
        }

        /// <summary>
        /// Free the reference to the backing stream.
        /// </summary>
        public void FreeReference()
        {
            if (!IsNull)
            {
                RefCounter.Decrease();
                if (RefCounter.IsZero)
                {
                    Stream.Dispose();
                }
                Stream = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            FreeReference();
            base.Dispose(disposing);
        }

        public override bool CanRead
        {
            get { return Stream != null && Stream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return Stream != null && Stream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return Stream != null && Stream.CanWrite; }
        }

        public override long Position
        {
            get { return Stream != null ? Stream.Position : 0; }
            set
            {
                ThrowIfNull();
                Stream.Position = value;
            }
        }

        public override long Length
        {
            get { return Stream != null ? Stream.Length : 0; }
        }

        /// <summary>
        /// The type of stream backing this SmartStream.
        /// </summary>
        public SmartStreamType StreamType
        {
            get
            {
                if (Stream == null)
                    return SmartStreamType.Null;
                if (Stream is MemoryStream)
                    return SmartStreamType.Memory;
                if (Stream is FileStream)
                    return SmartStreamType.File;
                return SmartStreamType.Other;
            }
        }

        /// <summary>
        /// Write the contents to a byte array.
        /// </summary>
        public byte[] ToArray()
        {
            ThrowIfNull();
            if (Stream is MemoryStream memoryStream)
            {
                return memoryStream.ToArray();
            }

            long initialPosition = Stream.Position;
            Stream.Position = 0;
            byte[] data = new byte[Stream.Length];
            Stream.Read(data, 0, data.Length);
            Stream.Position = initialPosition;
            return data;
        }

        private void ThrowIfNull()
        {
            if (IsNull)
            {
                throw new NullReferenceException("Stream is null");
            }
        }

        /// <summary>
        /// If true, this has no backing stream.
        /// </summary>
        public bool IsNull
        {
            get { return Stream == null; }
        }

        /// <summary>
        /// The number of references to the backing stream.
        /// </summary>
        public int RefCount
        {
            get { return RefCounter.RefCount; }
        }

        private SmartRefCount RefCounter { get; set; }
        private Stream Stream { get; set; }
    }
}
