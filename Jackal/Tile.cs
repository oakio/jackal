namespace Jackal
{
    public class Tile
    {
        public TileType Type;
        public int Coins;

        public Tile(TileType type, int coins = 0)
        {
            Type = type;
            Coins = coins;
        }
    }
}