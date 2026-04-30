using System;

namespace AssetStudio.Export
{
    /// <summary>
    /// Interface for asset exporters.
    /// Implementations handle exporting specific asset types.
    /// </summary>
    public interface IAssetExporter
    {
        /// <summary>
        /// Determines if this exporter can export the given asset type.
        /// </summary>
        /// <param name="type">The asset type to check.</param>
        /// <returns>True if this exporter can handle the type.</returns>
        bool CanExport(ClassIDType type);

        /// <summary>
        /// Exports a single asset.
        /// </summary>
        /// <param name="asset">The asset to export.</param>
        /// <param name="exportPath">The directory to export to.</param>
        /// <param name="options">Export options.</param>
        /// <returns>True if export was successful.</returns>
        bool Export(Object asset, string exportPath, ExportOptions options);

        /// <summary>
        /// Gets the file extension for the exported asset.
        /// </summary>
        /// <param name="asset">The asset to get extension for.</param>
        /// <param name="options">Export options.</param>
        /// <returns>The file extension including the dot (e.g., ".png").</returns>
        string GetFileExtension(Object asset, ExportOptions options);

        /// <summary>
        /// Gets the export type for this exporter.
        /// </summary>
        AssetType ExportType { get; }
    }
}
