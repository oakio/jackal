using Jackal;

namespace JackalHost.Actions
{
    class Explore : GameAction
    {
        private readonly Position _position;
        private readonly MapGenerator _generator;

        public Explore(Board board, Position position, MapGenerator generator) : base(board)
        {
            _position = position;
            _generator = generator;
        }

        public override void Act()
        {
            Board.Map[_position.X, _position.Y] = _generator.GetNext();
        }
    }
}