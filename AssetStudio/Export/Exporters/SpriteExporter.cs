using System;
using System.IO;

namespace AssetStudio.Export.Exporters
{
    /// <summary>
    /// Exporter for Sprite assets.
    /// Note: Full sprite extraction requires AssetStudioUtility.
    /// </summary>
    public class SpriteExporter : ExporterBase
    {
        public override AssetType ExportType => AssetType.Image;

        public override bool CanExport(ClassIDType type)
        {
            return type == ClassIDType.Sprite;
        }

        public override string GetFileExtension(Object asset, ExportOptions options)
        {
            return "." + options.ImageFormat.ToString().ToLower();
        }

        public override bool Export(Object asset, string exportPath, ExportOptions options)
        {
            var sprite = asset as Sprite;
            if (sprite == null)
            {
                return false;
            }

            // Full sprite extraction requires extension methods from AssetStudioUtility
            // This is a placeholder - actual extraction should be done in GUI layer
            string extension = GetFileExtension(asset, options);
            string fileName = FixFileName(sprite.m_Name);
            string filePath = GetUniqueFilePath(exportPath, fileName, extension, asset.m_PathID.ToString());

            // Export raw data as placeholder
            var data = sprite.GetRawData();
            if (data != null && data.Length > 0)
            {
                File.WriteAllBytes(filePath + ".raw", data);
            }

            return false;
        }
    }
}
