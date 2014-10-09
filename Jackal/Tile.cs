using System.Collections.Generic;

namespace Jackal
{
    public class Tile
    {
        public TileType Type;
        public int Coins;

        public int? OccupationTeamId;
        public HashSet<Pirate> Pirates;

        public Tile(TileType type, int coins = 0)
        {
            Type = type;
            Coins = coins;

            OccupationTeamId = null; // free
            Pirates = new HashSet<Pirate>();
        }
    }
}