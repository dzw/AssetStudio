using System;

namespace AssetStudio
{
    /// <summary>
    /// Base interface for all Unity objects.
    /// Provides common properties and methods shared across all asset types.
    /// </summary>
    public interface IUnityObjectBase
    {
        /// <summary>
        /// The path ID of this object within its serialized file.
        /// </summary>
        long PathID { get; }

        /// <summary>
        /// The class ID type of this object.
        /// </summary>
        ClassIDType ClassID { get; }

        /// <summary>
        /// The serialized file this object belongs to.
        /// </summary>
        SerializedFile AssetsFile { get; }

        /// <summary>
        /// The Unity version this object was created with.
        /// </summary>
        int[] Version { get; }

        /// <summary>
        /// The build target platform.
        /// </summary>
        BuildTarget Platform { get; }

        /// <summary>
        /// Gets the raw binary data of this object.
        /// </summary>
        /// <returns>The raw bytes of the object.</returns>
        byte[] GetRawData();

        /// <summary>
        /// Dumps the object to a string representation.
        /// </summary>
        /// <returns>The string dump of the object.</returns>
        string Dump();
    }

    /// <summary>
    /// Interface for named Unity objects.
    /// </summary>
    public interface INamedObject : IUnityObjectBase
    {
        /// <summary>
        /// The name of the object.
        /// </summary>
        string Name { get; }
    }

    /// <summary>
    /// Interface for Unity objects that contain assets.
    /// </summary>
    public interface IAsset : INamedObject
    {
        /// <summary>
        /// Whether this asset is a main asset.
        /// </summary>
        bool IsMainAsset { get; }

        /// <summary>
        /// The container path of this asset.
        /// </summary>
        string Container { get; }
    }

    /// <summary>
    /// Interface for Unity objects that can be previewed.
    /// </summary>
    public interface IPreviewable
    {
        /// <summary>
        /// Gets a preview image of the object.
        /// </summary>
        /// <returns>The preview image data, or null if not available.</returns>
        byte[] GetPreview();
    }

    /// <summary>
    /// Interface for Unity objects that can be exported.
    /// </summary>
    public interface IExportable
    {
        /// <summary>
        /// Exports the object to the specified path.
        /// </summary>
        /// <param name="path">The export path.</param>
        /// <param name="options">Export options.</param>
        /// <returns>True if export was successful.</returns>
        bool Export(string path, Export.ExportOptions options);
    }

    /// <summary>
    /// Interface for Unity objects that have dimensions.
    /// </summary>
    public interface IDimensional
    {
        /// <summary>
        /// The width of the object.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// The height of the object.
        /// </summary>
        int Height { get; }
    }

    /// <summary>
    /// Interface for Unity objects that have audio data.
    /// </summary>
    public interface IAudioObject
    {
        /// <summary>
        /// The number of audio channels.
        /// </summary>
        int Channels { get; }

        /// <summary>
        /// The audio frequency/sample rate.
        /// </summary>
        int Frequency { get; }

        /// <summary>
        /// The audio data.
        /// </summary>
        byte[] AudioData { get; }
    }

    /// <summary>
    /// Interface for Unity objects that have mesh data.
    /// </summary>
    public interface IMeshObject
    {
        /// <summary>
        /// The number of vertices.
        /// </summary>
        int VertexCount { get; }

        /// <summary>
        /// The vertices array.
        /// </summary>
        float[] Vertices { get; }

        /// <summary>
        /// The normals array.
        /// </summary>
        float[] Normals { get; }

        /// <summary>
        /// The UV array.
        /// </summary>
        float[] UV { get; }

        /// <summary>
        /// The indices array.
        /// </summary>
        uint[] Indices { get; }
    }
}
