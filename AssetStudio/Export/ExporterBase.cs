using System;
using System.IO;

namespace AssetStudio.Export
{
    /// <summary>
    /// Base class for asset exporters with common functionality.
    /// </summary>
    public abstract class ExporterBase : IAssetExporter
    {
        public abstract bool CanExport(ClassIDType type);
        public abstract bool Export(Object asset, string exportPath, ExportOptions options);
        public abstract string GetFileExtension(Object asset, ExportOptions options);
        public abstract AssetType ExportType { get; }

        /// <summary>
        /// Creates a unique file path for export.
        /// </summary>
        protected string GetUniqueFilePath(string directory, string fileName, string extension, string uniqueId)
        {
            Directory.CreateDirectory(directory);
            string filePath = Path.Combine(directory, fileName + extension);

            if (!File.Exists(filePath))
            {
                return filePath;
            }

            filePath = Path.Combine(directory, fileName + uniqueId + extension);
            if (!File.Exists(filePath))
            {
                return filePath;
            }

            // Generate random filename as last resort
            return Path.Combine(directory, Path.GetRandomFileName() + extension);
        }

        /// <summary>
        /// Fixes a filename by replacing invalid characters.
        /// </summary>
        protected string FixFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return "unnamed";
            }

            if (fileName.Length >= 260)
            {
                return Path.GetRandomFileName();
            }

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }

            return fileName;
        }
    }
}
