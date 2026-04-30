using System;
using System.IO;

namespace AssetStudio.Streams
{
    /// <summary>
    /// Read a slice of a file using a SafeFileHandle directly with System.IO.RandomAccess.
    /// This allows a container file with multiple logical files (eg, AssetBundles)
    /// to be streamed as if they are separate files, without buffering the entire file at once.
    /// </summary>
    internal sealed class RandomAccessStream : Stream
    {
        public FileStream Parent { get; }
        public long BaseOffset { get; }

        private long m_position;

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => false;
        public override long Length { get; }

        public override long Position
        {
            get => m_position - BaseOffset;
            set => m_position = value + BaseOffset;
        }

        public RandomAccessStream(FileStream parent, long offset, long length)
        {
            if (parent.Length < offset + length)
            {
                throw new ArgumentException("The parent stream is not long enough for the given offset and length.");
            }
            Parent = parent;
            BaseOffset = offset;
            Length = length;
            m_position = BaseOffset;
        }

        public override void Flush()
        {
            // Read-only streams shouldn't flush.
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            long toRead = Math.Min(count, Length - Position);
            Parent.Position = m_position;
            int read = Parent.Read(buffer, offset, (int)toRead);
            m_position += read;
            return read;
        }

        public override int ReadByte()
        {
            if (Position >= Length)
            {
                return -1;
            }
            Parent.Position = m_position;
            int result = Parent.ReadByte();
            if (result >= 0)
            {
                m_position++;
            }
            return result;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Current:
                    Position += offset;
                    break;
                case SeekOrigin.Begin:
                    Position = offset;
                    break;
                case SeekOrigin.End:
                    m_position = (Length - offset) + BaseOffset;
                    break;
            }
            return Position;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        ~RandomAccessStream()
        {
            GC.KeepAlive(Parent);
        }
    }
}
