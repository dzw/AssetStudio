namespace AssetStudio
{
    /// <summary>
    /// Represents a Visual Effect (VFX Graph) asset.
    /// ClassID: 2083052967
    /// </summary>
    public sealed class VisualEffect : NamedObject
    {
        public PPtr<VisualEffectAsset> m_VFXAsset;
        public bool m_CompileOnStartup;

        public VisualEffect(ObjectReader reader) : base(reader)
        {
            m_VFXAsset = new PPtr<VisualEffectAsset>(reader);
            m_CompileOnStartup = reader.ReadBoolean();
            reader.AlignStream();
        }
    }

    /// <summary>
    /// Represents a Visual Effect Asset resource.
    /// ClassID: 2058629509
    /// </summary>
    public sealed class VisualEffectAsset : NamedObject
    {
        public byte[] m_VFXData;

        public VisualEffectAsset(ObjectReader reader) : base(reader)
        {
            // Read VFX data from the asset
            var dataSize = reader.ReadInt32();
            if (dataSize > 0)
            {
                m_VFXData = reader.ReadBytes(dataSize);
            }
        }
    }
}
