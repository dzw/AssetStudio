using System.Collections.Generic;

namespace AssetStudio
{
    /// <summary>
    /// Represents a Tilemap component.
    /// ClassID: 1839735485
    /// </summary>
    public sealed class Tilemap : NamedObject
    {
        public PPtr<Grid> m_Grid;
        public int m_OriginX;
        public int m_OriginY;
        public int m_OriginZ;
        public int m_SizeX;
        public int m_SizeY;
        public int m_SizeZ;
        public List<TileInfo> m_Tiles;

        public Tilemap(ObjectReader reader) : base(reader)
        {
            m_Grid = new PPtr<Grid>(reader);
            m_OriginX = reader.ReadInt32();
            m_OriginY = reader.ReadInt32();
            m_OriginZ = reader.ReadInt32();
            m_SizeX = reader.ReadInt32();
            m_SizeY = reader.ReadInt32();
            m_SizeZ = reader.ReadInt32();

            var tileCount = reader.ReadInt32();
            m_Tiles = new List<TileInfo>(tileCount);
            for (int i = 0; i < tileCount; i++)
            {
                m_Tiles.Add(new TileInfo(reader));
            }
        }
    }

    /// <summary>
    /// Represents tile information in a tilemap.
    /// </summary>
    public sealed class TileInfo
    {
        public int m_X;
        public int m_Y;
        public int m_Z;
        public PPtr<TileBase> m_Tile;

        public TileInfo(ObjectReader reader)
        {
            m_X = reader.ReadInt32();
            m_Y = reader.ReadInt32();
            m_Z = reader.ReadInt32();
            m_Tile = new PPtr<TileBase>(reader);
        }
    }

    /// <summary>
    /// Base class for tiles.
    /// ClassID: 687078895 (same as SpriteAtlas, different subclasses)
    /// </summary>
    public class TileBase : NamedObject
    {
        public TileBase(ObjectReader reader) : base(reader)
        {
        }
    }

    /// <summary>
    /// Represents a Grid component.
    /// ClassID: -1 (internal type)
    /// </summary>
    public sealed class Grid : NamedObject
    {
        public float m_CellSizeX;
        public float m_CellSizeY;
        public float m_CellSizeZ;
        public int m_CellLayout;
        public int m_CellSwizzle;

        public Grid(ObjectReader reader) : base(reader)
        {
            m_CellSizeX = reader.ReadSingle();
            m_CellSizeY = reader.ReadSingle();
            m_CellSizeZ = reader.ReadSingle();
            m_CellLayout = reader.ReadInt32();
            m_CellSwizzle = reader.ReadInt32();
        }
    }
}
