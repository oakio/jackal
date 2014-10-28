namespace Jackal
{
    public class Pirate
    {
        public int TeamId;
        public Position Position;
        public bool IsDrunk;

        internal int? DrunkSinceTurnNo;

        
        public Pirate(int teamId, Position position)
        {
            TeamId = teamId;
            Position = position;
        }
    }
}