using System.Collections.Generic;

namespace AssetStudio
{
    /// <summary>
    /// Represents a Prefab asset.
    /// ClassID: 1001480554
    /// </summary>
    public sealed class Prefab : NamedObject
    {
        public PPtr<GameObject> m_RootGameObject;
        public List<PPtr<Object>> m_Modification;
        public bool m_IsPrefabAsset;

        public Prefab(ObjectReader reader) : base(reader)
        {
            m_RootGameObject = new PPtr<GameObject>(reader);

            var modCount = reader.ReadInt32();
            m_Modification = new List<PPtr<Object>>(modCount);
            for (int i = 0; i < modCount; i++)
            {
                m_Modification.Add(new PPtr<Object>(reader));
            }

            m_IsPrefabAsset = reader.ReadBoolean();
            reader.AlignStream();
        }
    }

    /// <summary>
    /// Represents a Prefab Instance.
    /// ClassID: 1001
    /// </summary>
    public sealed class PrefabInstance : NamedObject
    {
        public PPtr<Prefab> m_SourcePrefab;
        public PPtr<GameObject> m_RootGameObject;
        public long m_ModificationHash;

        public PrefabInstance(ObjectReader reader) : base(reader)
        {
            m_SourcePrefab = new PPtr<Prefab>(reader);
            m_RootGameObject = new PPtr<GameObject>(reader);
            m_ModificationHash = reader.ReadInt64();
        }
    }
}
