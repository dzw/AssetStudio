using System;
using System.IO;

namespace AssetStudio.Export.Exporters
{
    /// <summary>
    /// Exporter for AudioClip assets.
    /// Note: This is a basic implementation. Full conversion requires AssetStudioUtility.
    /// </summary>
    public class AudioClipExporter : ExporterBase
    {
        public override AssetType ExportType => AssetType.Audio;

        public override bool CanExport(ClassIDType type)
        {
            return type == ClassIDType.AudioClip;
        }

        public override string GetFileExtension(Object asset, ExportOptions options)
        {
            var audioClip = asset as AudioClip;
            if (audioClip == null)
            {
                return ".audio";
            }

            if (options.ConvertAudio)
            {
                return ".wav";
            }

            return GetOriginalExtension(audioClip);
        }

        private string GetOriginalExtension(AudioClip audioClip)
        {
            switch (audioClip.m_CompressionFormat)
            {
                case AudioCompressionFormat.PCM:
                    return ".wav";
                case AudioCompressionFormat.Vorbis:
                    return ".ogg";
                case AudioCompressionFormat.ADPCM:
                    return ".wav";
                case AudioCompressionFormat.MP3:
                    return ".mp3";
                case AudioCompressionFormat.XMA:
                    return ".xma";
                case AudioCompressionFormat.AAC:
                    return ".m4a";
                case AudioCompressionFormat.GCADPCM:
                    return ".wav";
                case AudioCompressionFormat.ATRAC9:
                    return ".at9";
                default:
                    return ".fsb";
            }
        }

        public override bool Export(Object asset, string exportPath, ExportOptions options)
        {
            var audioClip = asset as AudioClip;
            if (audioClip == null)
            {
                return false;
            }

            var audioData = audioClip.m_AudioData.GetData();
            if (audioData == null || audioData.Length == 0)
            {
                return false;
            }

            string extension = GetFileExtension(asset, options);
            string fileName = FixFileName(audioClip.m_Name);
            string filePath = GetUniqueFilePath(exportPath, fileName, extension, asset.m_PathID.ToString());

            // Export original format - WAV conversion requires AssetStudioUtility
            File.WriteAllBytes(filePath, audioData);
            return true;
        }
    }
}
