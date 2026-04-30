using System;
using System.IO;

namespace AssetStudio.Export.Exporters
{
    /// <summary>
    /// Exporter for Font assets.
    /// </summary>
    public class FontExporter : ExporterBase
    {
        public override AssetType ExportType => AssetType.Native;

        public override bool CanExport(ClassIDType type)
        {
            return type == ClassIDType.Font;
        }

        public override string GetFileExtension(Object asset, ExportOptions options)
        {
            var font = asset as Font;
            if (font?.m_FontData != null && font.m_FontData.Length >= 4)
            {
                // Check for OTF signature
                if (font.m_FontData[0] == 79 && font.m_FontData[1] == 84 &&
                    font.m_FontData[2] == 84 && font.m_FontData[3] == 79)
                {
                    return ".otf";
                }
            }
            return ".ttf";
        }

        public override bool Export(Object asset, string exportPath, ExportOptions options)
        {
            var font = asset as Font;
            if (font == null || font.m_FontData == null)
            {
                return false;
            }

            string extension = GetFileExtension(asset, options);
            string fileName = FixFileName(font.m_Name);
            string filePath = GetUniqueFilePath(exportPath, fileName, extension, asset.m_PathID.ToString());

            File.WriteAllBytes(filePath, font.m_FontData);
            return true;
        }
    }
}
