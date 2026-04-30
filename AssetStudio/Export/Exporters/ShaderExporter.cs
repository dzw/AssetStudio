using System;
using System.IO;

namespace AssetStudio.Export.Exporters
{
    /// <summary>
    /// Exporter for Shader assets.
    /// Note: Full shader decompilation requires AssetStudioUtility.
    /// </summary>
    public class ShaderExporter : ExporterBase
    {
        public override AssetType ExportType => AssetType.Shader;

        public override bool CanExport(ClassIDType type)
        {
            return type == ClassIDType.Shader;
        }

        public override string GetFileExtension(Object asset, ExportOptions options)
        {
            return ".shader";
        }

        public override bool Export(Object asset, string exportPath, ExportOptions options)
        {
            var shader = asset as Shader;
            if (shader == null)
            {
                return false;
            }

            string fileName = FixFileName(shader.m_Name);
            string filePath = GetUniqueFilePath(exportPath, fileName, ".shader", asset.m_PathID.ToString());

            // Export raw shader data - full decompilation requires AssetStudioUtility
            var data = shader.GetRawData();
            if (data != null && data.Length > 0)
            {
                File.WriteAllBytes(filePath, data);
                return true;
            }

            // Write placeholder
            File.WriteAllText(filePath, $"// Shader: {shader.m_Name}\n// Decompile with AssetStudioGUI\n");
            return true;
        }
    }
}
