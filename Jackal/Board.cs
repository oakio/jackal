namespace Jackal
{
    public class Board
    {
        public const int Size = 13;

        public Tile[,] Map;
        public Team[] Teams;

        public Board()
        {
            Map = new Tile[Size, Size];
            InitWater();

            Teams = new Team[4];
            InitTeam(0, (Size - 1)/2, 0);
            InitTeam(1, (Size - 1), (Size - 1)/2);
            InitTeam(2, (Size - 1)/2, (Size - 1));
            InitTeam(3, 0, (Size - 1)/2);
        }

        private void InitWater()
        {
            for (int i = 0; i < Size; i++)
            {
                Map[i, 0] = new Tile(TileType.Water);
                Map[0, i] = new Tile(TileType.Water);
                Map[i, Size - 1] = new Tile(TileType.Water);
                Map[Size - 1, i] = new Tile(TileType.Water);
            }
        }

        private void InitTeam(int teamId, int x, int y)
        {
            var startPosition = new Position(x, y);
            var ship = new Ship(teamId, startPosition);
            var pirates = new Pirate[3];
            for (int i = 0; i < pirates.Length; i++)
            {
                pirates[i] = new Pirate(teamId, startPosition);
            }
            Teams[teamId] = new Team(teamId, ship, pirates);
        }
    }
}