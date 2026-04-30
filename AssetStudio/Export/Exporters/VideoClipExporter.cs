using System;
using System.IO;

namespace AssetStudio.Export.Exporters
{
    /// <summary>
    /// Exporter for VideoClip assets.
    /// </summary>
    public class VideoClipExporter : ExporterBase
    {
        public override AssetType ExportType => AssetType.Video;

        public override bool CanExport(ClassIDType type)
        {
            return type == ClassIDType.VideoClip;
        }

        public override string GetFileExtension(Object asset, ExportOptions options)
        {
            var videoClip = asset as VideoClip;
            if (videoClip != null && !string.IsNullOrEmpty(videoClip.m_OriginalPath))
            {
                return Path.GetExtension(videoClip.m_OriginalPath);
            }
            return ".mp4";
        }

        public override bool Export(Object asset, string exportPath, ExportOptions options)
        {
            var videoClip = asset as VideoClip;
            if (videoClip == null)
            {
                return false;
            }

            if (videoClip.m_ExternalResources.m_Size <= 0)
            {
                return false;
            }

            string extension = GetFileExtension(asset, options);
            string fileName = FixFileName(videoClip.m_Name);
            string filePath = GetUniqueFilePath(exportPath, fileName, extension, asset.m_PathID.ToString());

            videoClip.m_VideoData.WriteData(filePath);
            return true;
        }
    }
}
