using Jackal;

namespace JackalHost.Actions
{
    class Explore : IGameAction
    {
        private readonly Position _position;
        
        public Explore(Position position)
        {
            _position = position;
        }

        public void Act(Game game)
        {
            Board board = game.Board;
            board.Map[_position.X, _position.Y] = board.Generator.GetNext();
        }
    }
}