using System.Collections.Generic;

namespace Jackal
{
    public class Ship
    {
        public int TeamId;
        public Position Position;
        public int Coins;
        public HashSet<Pirate> Crew;

        public Ship(int teamId, Position position, HashSet<Pirate> crew)
        {
            TeamId = teamId;
            Position = position;
            Coins = 0;
            Crew = crew;
        }
    }
}