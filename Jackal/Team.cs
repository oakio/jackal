namespace Jackal
{
    public class Team
    {
        public int Id;
        public Ship Ship;
        public Pirate[] Pirates;
        public int[] Enemies;

        public Team(int id, Ship ship, Pirate[] pirates)
        {
            Id = id;
            Ship = ship;
            Pirates = pirates;
        }
    }
}