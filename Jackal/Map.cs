namespace Jackal
{
    public class Map
    {
        private Tile[,] map=new Tile[Board.Size,Board.Size];

        public bool AirplaneUsed;

        public Tile this[Position pos]
        {
            get { return map[pos.X, pos.Y]; }
            set { map[pos.X, pos.Y] = value; }
        }
        public Tile this[int x, int y]  
        {
            get { return map[x, y]; }
            set { map[x, y] = value; }
        }
    }
}