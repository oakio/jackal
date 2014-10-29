using System;
using System.Collections.Generic;
using System.Linq;

namespace Jackal
{
    public class MapGenerator
    {
        public const int Size = 11;
        public const int TotalCoins = 37;

        private readonly Random _rand;
        private readonly List<Tile> _tiles;

        public MapGenerator(int mapId)
        {
            _rand = new Random(mapId);

            const int totalUnknown = Size*Size - 4;
            List<Tile> tiles = new List<Tile>(totalUnknown);

            for (int i = 0; i < 5; i++)
            {
                tiles.Add(new Tile(TileType.Chest1, 1));
                tiles.Add(new Tile(TileType.Chest2, 2));
            }

            for (int i = 0; i < 3; i++)
            {
                tiles.Add(new Tile(TileType.Chest3, 3));
            }

            for (int i = 0; i < 2; i++)
            {
                tiles.Add(new Tile(TileType.Chest4, 4));
            }

            tiles.Add(new Tile(TileType.Chest5, 5));

            for (int i = 0; i < 2; i++)
            {
                tiles.Add(new Tile(TileType.Fort));
            }
            tiles.Add(new Tile(TileType.RespawnFort));

            for (int i = 0; i < 4; i++)
            {
                tiles.Add(new Tile(TileType.RumBarrel));
            }


            for (int i = 0; i < 2; i++)
            {
                tiles.Add(new Tile(TileType.Horse));
            }

            //arrows
            for (int i = 0; i < 3; i++)
            {
                for (int arrowType = 0; arrowType < ArrowsCodesHelper.ArrowsTypes.Length; arrowType++)
                {
                    int arrowsCode = ArrowsCodesHelper.ArrowsTypes[arrowType];
                    int rotatesCount = _rand.Next(4);
                    for (int j = 1; j <= rotatesCount; j++)
                    {
                        arrowsCode = ArrowsCodesHelper.DoRotate(arrowsCode);
                    }
                    var tile = new Tile(TileType.Arrow);
                    tile.ArrowsCode = arrowsCode;
                    tiles.Add(tile);
                }
            }

            while (tiles.Count < totalUnknown)
            {
                tiles.Add(new Tile(TileType.Grass));
            }

            _tiles = Shuffle(tiles);
        }

        Tile NewArrow(string str)
        {
            int arrowsCode = ArrowsCodesHelper.GetCodeFromString(str);
            int rotatesCount= _rand.Next(4);
            for (int i = 1; i <= rotatesCount; i++)
            {
                arrowsCode = ArrowsCodesHelper.DoRotate(arrowsCode);
            }
            var tile=new Tile(TileType.Arrow);
            tile.ArrowsCode = arrowsCode;
            return tile;
        }

        private List<Tile> Shuffle( List<Tile> tiles)
        {
            return tiles.Select(x => new {Tile = x, Number = _rand.Next()}).OrderBy(x => x.Number).Select(x => x.Tile).ToList();
        }

        public Tile GetNext(Position position)
        {
            int index = (position.Y - 1)*11 + position.X - 3;
            if (position.Y == 1)
                index++;
            if (position.Y == 11)
                index--;

            return _tiles[index];
        }
    }
}