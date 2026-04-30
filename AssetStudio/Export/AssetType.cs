namespace AssetStudio.Export
{
    /// <summary>
    /// Represents the type of export for an asset.
    /// </summary>
    public enum AssetType
    {
        /// <summary>
        /// The asset type is unknown.
        /// </summary>
        Unknown,
        /// <summary>
        /// The asset will be exported as its native format.
        /// </summary>
        Native,
        /// <summary>
        /// The asset will be exported as a text format.
        /// </summary>
        Text,
        /// <summary>
        /// The asset will be exported as a binary format.
        /// </summary>
        Binary,
        /// <summary>
        /// The asset will be exported as an image format.
        /// </summary>
        Image,
        /// <summary>
        /// The asset will be exported as an audio format.
        /// </summary>
        Audio,
        /// <summary>
        /// The asset will be exported as a model format (FBX, GLB, etc).
        /// </summary>
        Model,
        /// <summary>
        /// The asset will be exported as a video format.
        /// </summary>
        Video,
        /// <summary>
        /// The asset will be exported as a prefab.
        /// </summary>
        Prefab,
        /// <summary>
        /// The asset will be exported as a scene.
        /// </summary>
        Scene,
        /// <summary>
        /// The asset will be exported as a script.
        /// </summary>
        Script,
        /// <summary>
        /// The asset will be exported as a shader.
        /// </summary>
        Shader,
        /// <summary>
        /// The asset will be exported as JSON.
        /// </summary>
        Json,
    }
}
