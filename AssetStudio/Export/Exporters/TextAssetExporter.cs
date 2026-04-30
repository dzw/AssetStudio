using System;
using System.IO;

namespace AssetStudio.Export.Exporters
{
    /// <summary>
    /// Exporter for TextAsset assets.
    /// </summary>
    public class TextAssetExporter : ExporterBase
    {
        public override AssetType ExportType => AssetType.Text;

        public override bool CanExport(ClassIDType type)
        {
            return type == ClassIDType.TextAsset;
        }

        public override string GetFileExtension(Object asset, ExportOptions options)
        {
            var textAsset = asset as TextAsset;
            if (textAsset == null)
            {
                return ".txt";
            }

            if (options.RestoreExtensionName && !string.IsNullOrEmpty(textAsset.m_Name))
            {
                // Try to detect extension from content
                return DetectExtension(textAsset.m_Script);
            }

            return ".txt";
        }

        private string DetectExtension(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return ".txt";
            }

            // Check for common file signatures
            if (data.Length >= 3 && data[0] == 0xEF && data[1] == 0xBB && data[2] == 0xBF)
            {
                return ".txt"; // UTF-8 BOM
            }

            if (data.Length >= 4)
            {
                // PNG
                if (data[0] == 0x89 && data[1] == 0x50 && data[2] == 0x4E && data[3] == 0x47)
                {
                    return ".png";
                }
                // JPEG
                if (data[0] == 0xFF && data[1] == 0xD8 && data[2] == 0xFF)
                {
                    return ".jpg";
                }
                // ZIP/JAR
                if (data[0] == 0x50 && data[1] == 0x4B)
                {
                    return ".zip";
                }
                // PDF
                if (data[0] == 0x25 && data[1] == 0x50 && data[2] == 0x44 && data[3] == 0x46)
                {
                    return ".pdf";
                }
            }

            // Check for JSON
            string start = System.Text.Encoding.UTF8.GetString(data, 0, Math.Min(data.Length, 10)).TrimStart();
            if (start.StartsWith("{") || start.StartsWith("["))
            {
                return ".json";
            }

            // Check for XML
            if (start.StartsWith("<"))
            {
                return ".xml";
            }

            return ".txt";
        }

        public override bool Export(Object asset, string exportPath, ExportOptions options)
        {
            var textAsset = asset as TextAsset;
            if (textAsset == null)
            {
                return false;
            }

            string extension = GetFileExtension(asset, options);
            string fileName = FixFileName(textAsset.m_Name);
            string filePath = GetUniqueFilePath(exportPath, fileName, extension, asset.m_PathID.ToString());

            File.WriteAllBytes(filePath, textAsset.m_Script);
            return true;
        }
    }
}
