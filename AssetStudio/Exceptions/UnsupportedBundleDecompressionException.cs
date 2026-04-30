using System;

namespace AssetStudio.Exceptions
{
    public sealed class UnsupportedBundleDecompressionException : NotSupportedException
    {
        private UnsupportedBundleDecompressionException(string message) : base(message) { }

        public static void ThrowLzham(string fileName)
        {
            throw new UnsupportedBundleDecompressionException($"Lzham decompression is not currently supported. File: {fileName}");
        }

        public static void Throw(string fileName, string compression)
        {
            throw new UnsupportedBundleDecompressionException($"Bundle compression '{compression}' is not supported. '{fileName}' is likely encrypted or using a custom compression algorithm.");
        }
    }
}
