using System;

namespace AssetStudio.Exceptions
{
    public sealed class InvalidFormatException : Exception
    {
        public InvalidFormatException(string message) : base(message)
        {
        }
    }
}
