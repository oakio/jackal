using System;
using System.Collections.Generic;
using System.Linq;
using Jackal;

namespace JackalHost
{
    public class MapGenerator
    {
        public int TotalCoins = 37;

        static readonly Random Rnd = new Random(42);
        private readonly List<Tile> _tiles;
        private int _next;

        public MapGenerator()
        {
            _tiles = new List<Tile>(11*11);

            for (int i = 0; i < 5; i++)
            {
                _tiles.Add(new Tile(TileType.Grass) {Coins = 1});
                _tiles.Add(new Tile(TileType.Grass) {Coins = 2});
            }

            for (int i = 0; i < 3; i++)
            {
                _tiles.Add(new Tile(TileType.Grass) {Coins = 3});
            }

            for (int i = 0; i < 2; i++)
            {
                _tiles.Add(new Tile(TileType.Grass) {Coins = 4});
            }

            _tiles.Add(new Tile(TileType.Grass) {Coins = 5});

            for (int i = 0; i < 110; i++)
            {
                _tiles.Add(new Tile(TileType.Grass));
            }
            _tiles = _tiles.OrderBy(x => Rnd.Next()).ToList();
        }

        public Tile GetNext()
        {
            return _tiles[_next++];
        }
    }
}