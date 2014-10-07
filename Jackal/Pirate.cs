namespace Jackal
{
    public class Pirate
    {
        public int TeamId;
        public Position Position;
        public int Coins;

        public Pirate(int teamId, Position position)
        {
            TeamId = teamId;
            Position = position;
            Coins = 0;
        }
    }
}