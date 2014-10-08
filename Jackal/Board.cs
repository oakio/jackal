using System.Collections.Generic;

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
            InitMap();


            Teams = new Team[4];
            InitTeam(0, (Size - 1)/2, 0);
            InitTeam(1, (Size - 1), (Size - 1)/2);
            InitTeam(2, (Size - 1)/2, (Size - 1));
            InitTeam(3, 0, (Size - 1)/2);

            Teams[0].Enemies = new[] {Teams[1], Teams[2], Teams[3]};
            Teams[1].Enemies = new[] {Teams[0], Teams[2], Teams[3]};
            Teams[2].Enemies = new[] {Teams[0], Teams[1], Teams[3]};
            Teams[3].Enemies = new[] {Teams[0], Teams[1], Teams[2]};
        }

        private void InitMap()
        {

            for (int i = 0; i < Size; i++)
            {
                SetWater(i, 0);
                SetWater(0, i);
                SetWater(i, Size - 1);
                SetWater(Size - 1, i);
            }

            for (int x = 1; x < Size - 1; x++)
            {
                for (int y = 1; y < Size - 1; y++)
                {
                    SetUnknown(x, y);
                }
            }
            SetStone(0, 0);
            SetStone(Size - 1, 0);
            SetStone(Size - 1, Size - 1);
            SetStone(0, Size - 1);
        }

        void SetWater(int x, int y)
        {
            Map[x, y] = new Tile(TileType.Water);
        }
        void SetUnknown(int x, int y)
        {
            Map[x, y] = new Tile(TileType.Unknown);
        }
        void SetStone(int x, int y)
        {
            Map[x, y] = new Tile(TileType.Stone);
        }

        private void InitTeam(int teamId, int x, int y)
        {
            var startPosition = new Position(x, y);
            var pirates = new Pirate[3];
            for (int i = 0; i < pirates.Length; i++)
            {
                pirates[i] = new Pirate(teamId, startPosition);
            }
            var ship = new Ship(teamId, startPosition, new HashSet<Pirate>(pirates));
            Teams[teamId] = new Team(teamId, ship, pirates);
        }
    }
}