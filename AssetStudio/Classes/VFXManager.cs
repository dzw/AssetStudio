namespace AssetStudio
{
    /// <summary>
    /// Represents a VFX Manager component.
    /// ClassID: 937362698
    /// </summary>
    public sealed class VFXManager : NamedObject
    {
        public float m_StopCullingDistance;
        public float m_StartCullingDistance;
        public int m_MaxCullingGranularity;

        public VFXManager(ObjectReader reader) : base(reader)
        {
            m_StopCullingDistance = reader.ReadSingle();
            m_StartCullingDistance = reader.ReadSingle();
            m_MaxCullingGranularity = reader.ReadInt32();
        }
    }
}
