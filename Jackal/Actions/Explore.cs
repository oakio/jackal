namespace Jackal.Actions
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
            var newTile = board.Generator.GetNext(_position);
            newTile.Position = new Position(_position.X, _position.Y);
            board.Map[_position.X, _position.Y] = newTile;
        }
    }
}