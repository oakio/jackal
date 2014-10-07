namespace Jackal
{
    public class Ship
    {
        public int TeamId;
        public Position Position;
        public int Coins;

        public Ship(int teamId, Position position)
        {
            TeamId = teamId;
            Position = position;
            Coins = 0;
        }
    }
}