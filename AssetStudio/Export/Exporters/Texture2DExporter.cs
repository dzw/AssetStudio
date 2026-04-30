using System;
using System.IO;

namespace AssetStudio.Export.Exporters
{
    /// <summary>
    /// Exporter for Texture2D assets.
    /// Note: This is a basic implementation. Full conversion requires AssetStudioUtility.
    /// </summary>
    public class Texture2DExporter : ExporterBase
    {
        public override AssetType ExportType => AssetType.Image;

        public override bool CanExport(ClassIDType type)
        {
            return type == ClassIDType.Texture2D;
        }

        public override string GetFileExtension(Object asset, ExportOptions options)
        {
            if (options.ConvertTexture)
            {
                return "." + options.ImageFormat.ToString().ToLower();
            }
            return ".tex";
        }

        public override bool Export(Object asset, string exportPath, ExportOptions options)
        {
            var texture = asset as Texture2D;
            if (texture == null)
            {
                return false;
            }

            string extension = GetFileExtension(asset, options);
            string fileName = FixFileName(texture.m_Name);
            string filePath = GetUniqueFilePath(exportPath, fileName, extension, asset.m_PathID.ToString());

            if (options.ConvertTexture)
            {
                // Full conversion requires extension methods from AssetStudioUtility
                // This is a placeholder - actual conversion should be done in GUI layer
                return false;
            }
            else
            {
                // Export raw texture data
                var data = texture.image_data.GetData();
                if (data == null || data.Length == 0)
                {
                    return false;
                }
                File.WriteAllBytes(filePath, data);
                return true;
            }
        }
    }
}
