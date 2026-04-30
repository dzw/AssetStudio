using System;

namespace AssetStudio.Export
{
    /// <summary>
    /// Options for asset export operations.
    /// </summary>
    public class ExportOptions
    {
        /// <summary>
        /// Image export format.
        /// </summary>
        public ImageFormat ImageFormat { get; set; } = ImageFormat.Png;

        /// <summary>
        /// Model export format.
        /// </summary>
        public ModelFormat ModelFormat { get; set; } = ModelFormat.Fbx;

        /// <summary>
        /// Audio export format.
        /// </summary>
        public AudioFormat AudioFormat { get; set; } = AudioFormat.Default;

        /// <summary>
        /// Scale factor for model export.
        /// </summary>
        public float ScaleFactor { get; set; } = 1.0f;

        /// <summary>
        /// Whether to apply euler filter to animations.
        /// </summary>
        public bool EulerFilter { get; set; } = true;

        /// <summary>
        /// Precision for euler filter.
        /// </summary>
        public float FilterPrecision { get; set; } = 0.25f;

        /// <summary>
        /// Whether to export all nodes in hierarchy.
        /// </summary>
        public bool ExportAllNodes { get; set; } = true;

        /// <summary>
        /// Whether to export skins/bones.
        /// </summary>
        public bool ExportSkins { get; set; } = true;

        /// <summary>
        /// Whether to export animations.
        /// </summary>
        public bool ExportAnimations { get; set; } = true;

        /// <summary>
        /// Whether to export blend shapes.
        /// </summary>
        public bool ExportBlendShape { get; set; } = true;

        /// <summary>
        /// Whether to cast bones automatically.
        /// </summary>
        public bool CastToBone { get; set; } = false;

        /// <summary>
        /// Bone size for automatic bone casting.
        /// </summary>
        public int BoneSize { get; set; } = 100;

        /// <summary>
        /// Whether to restore original extension names.
        /// </summary>
        public bool RestoreExtensionName { get; set; } = true;

        /// <summary>
        /// Whether to convert textures to image formats.
        /// </summary>
        public bool ConvertTexture { get; set; } = true;

        /// <summary>
        /// Whether to convert audio to wav format.
        /// </summary>
        public bool ConvertAudio { get; set; } = true;

        /// <summary>
        /// FBX version index.
        /// </summary>
        public int FbxVersion { get; set; } = 0;

        /// <summary>
        /// Whether to export FBX as ASCII.
        /// </summary>
        public bool FbxAscii { get; set; } = false;
    }

    /// <summary>
    /// Image export formats.
    /// </summary>
    public enum ImageFormat
    {
        Png,
        Jpeg,
        Bmp,
        Tga,
        Exr,
    }

    /// <summary>
    /// Model export formats.
    /// </summary>
    public enum ModelFormat
    {
        Fbx,
        Glb,
        GlbBinary,
        Obj,
    }

    /// <summary>
    /// Audio export formats.
    /// </summary>
    public enum AudioFormat
    {
        Default,
        Wav,
        Ogg,
        Mp3,
    }
}
