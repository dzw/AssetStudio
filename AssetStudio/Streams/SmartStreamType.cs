namespace AssetStudio.Streams
{
    public enum SmartStreamType
    {
        /// <summary>
        /// The SmartStream is not backed by a Stream.
        /// </summary>
        Null,
        /// <summary>
        /// The SmartStream is backed by a FileStream.
        /// </summary>
        File,
        /// <summary>
        /// The SmartStream is backed by a MemoryStream.
        /// </summary>
        Memory,
        /// <summary>
        /// The SmartStream is backed by another type of Stream.
        /// </summary>
        Other,
    }
}
