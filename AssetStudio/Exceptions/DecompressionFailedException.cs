using System;

namespace AssetStudio.Exceptions
{
    public sealed class DecompressionFailedException : Exception
    {
        private DecompressionFailedException(string message) : base(message) { }

        public static void ThrowNoBytesWritten(string fileName, string compression)
        {
            throw new DecompressionFailedException($"Could not write any bytes for '{fileName}' while decompressing {compression}. File: {fileName}");
        }

        public static void ThrowReadMoreThanExpected(string compression, long expected, long actual)
        {
            throw new DecompressionFailedException($"Read more than expected while decompressing {compression}. Expected {expected}, but was {actual}.");
        }

        public static void ThrowReadMoreThanExpectedForFile(string fileName, long expected, long actual)
        {
            throw new DecompressionFailedException($"Read more than expected for '{fileName}' while decompressing. Expected {expected}, but was {actual}.");
        }

        public static void ThrowIncorrectNumberBytesWritten(string fileName, string compression, long expected, long actual)
        {
            throw new DecompressionFailedException($"Incorrect number of bytes written for '{fileName}' while decompressing {compression}. Expected {expected}, but was {actual}.");
        }

        public static void ThrowIfUncompressedSizeIsNegative(string fileName, long uncompressedSize)
        {
            if (uncompressedSize < 0)
            {
                throw new DecompressionFailedException($"Uncompressed size cannot be negative: {uncompressedSize}. File: {fileName}");
            }
        }
    }
}
