using Jackal;

namespace JackalHost.Actions
{
    class TakeCoin : IGameAction
    {
        private readonly Pirate _pirate;

        public TakeCoin(Pirate pirate)
        {
            _pirate = pirate;
        }

        public void Act(Game game)
        {
            Board board = game.Board;
            Position position = _pirate.Position;
            Tile tile = board.Map[position.X, position.Y];
            if (tile.Coins == 0 || _pirate.Coins > 0)
            {
                return;
            }

            tile.Coins--;
            _pirate.Coins++;
        }
    }
}