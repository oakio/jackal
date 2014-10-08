using Jackal;

namespace JackalHost.Actions
{
    class TakeCoin : GameAction
    {
        private readonly Pirate _pirate;

        public TakeCoin(Board board, Pirate pirate) : base(board)
        {
            _pirate = pirate;
        }

        public override void Act()
        {
            Position position = _pirate.Position;
            Tile tile = Board.Map[position.X, position.Y];
            if (tile.Coins == 0 || _pirate.Coins > 0)
            {
                return;
            }

            tile.Coins--;
            _pirate.Coins++;
        }
    }
}