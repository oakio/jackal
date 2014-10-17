using System;
using System.Collections.Generic;
using System.Linq;

namespace Jackal
{
    public class MapGenerator
    {
        public int Size = 11;
        public int TotalCoins = 37;

        private readonly Random _rand;
        private readonly List<Tile> _tiles;
        private int _nextTile;

        public MapGenerator(int mapId)
        {
            _rand = new Random(mapId);

            int totalUnknown = Size*Size - 4;
            _tiles = new List<Tile>(totalUnknown);

            for (int i = 0; i < 5; i++)
            {
                AddTile(new Tile(TileType.Grass, 1));
                AddTile(new Tile(TileType.Grass, 2));
            }

            for (int i = 0; i < 3; i++)
            {
                AddTile(new Tile(TileType.Grass, 3));
            }

            for (int i = 0; i < 2; i++)
            {
                AddTile(new Tile(TileType.Grass, 4));
            }

            AddTile(new Tile(TileType.Grass, 5));

            while(_tiles.Count<totalUnknown)
            {
                AddTile(new Tile(TileType.Grass));
            }

            _tiles = ShuffleTiles();
        }

        private void AddTile(Tile tile)
        {
            _tiles.Add(tile);
        }

        private List<Tile> ShuffleTiles()
        {
            return _tiles.OrderBy(x => _rand.Next()).ToList();
        }

        public Tile GetNext()
        {
            return _tiles[_nextTile++];
        }
    }
}