using System.Collections.Generic;

namespace AssetStudio
{
    /// <summary>
    /// Represents a Localization Asset.
    /// ClassID: 2083778819
    /// </summary>
    public sealed class LocalizationAsset : NamedObject
    {
        public string m_LocaleIdentifier;
        public List<PPtr<Object>> m_LocalizedAssets;

        public LocalizationAsset(ObjectReader reader) : base(reader)
        {
            m_LocaleIdentifier = reader.ReadAlignedString();

            var assetCount = reader.ReadInt32();
            m_LocalizedAssets = new List<PPtr<Object>>(assetCount);
            for (int i = 0; i < assetCount; i++)
            {
                m_LocalizedAssets.Add(new PPtr<Object>(reader));
            }
        }
    }
}
