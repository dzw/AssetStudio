using System;
using System.IO;

namespace AssetStudio.Export.Exporters
{
    /// <summary>
    /// Fallback exporter for unhandled asset types.
    /// Exports the raw asset data.
    /// </summary>
    public class RawAssetExporter : ExporterBase
    {
        public override AssetType ExportType => AssetType.Native;

        public override bool CanExport(ClassIDType type)
        {
            return true; // Can export any type as raw data
        }

        public override string GetFileExtension(Object asset, ExportOptions options)
        {
            return ".dat";
        }

        public override bool Export(Object asset, string exportPath, ExportOptions options)
        {
            var data = asset.GetRawData();
            if (data == null || data.Length == 0)
            {
                return false;
            }

            string fileName = GetAssetName(asset);
            string filePath = GetUniqueFilePath(exportPath, fileName, ".dat", asset.m_PathID.ToString());

            File.WriteAllBytes(filePath, data);
            return true;
        }

        private string GetAssetName(Object asset)
        {
            // Try to get name from NamedObject derived types
            if (asset is NamedObject namedObj)
            {
                return FixFileName(namedObj.m_Name);
            }
            return "asset_" + asset.m_PathID;
        }
    }
}
