using System.Collections.Generic;

namespace Jackal
{
    public class Tile
    {
        public Position Position;
        public TileType Type;
        public int Coins;

        public int ArrowsCode;

        public int? OccupationTeamId;
        public HashSet<Pirate> Pirates;

        public Tile()
        {
        }

        public Tile(Position position,TileType type, int coins = 0):this(type,coins)
        {
            Position = position;
        }

        public Tile(TileType type, int coins = 0)
        {
            Type = type;
            Coins = coins;

            OccupationTeamId = null; // free
            Pirates = new HashSet<Pirate>();
        }
    }
}