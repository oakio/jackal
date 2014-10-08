using Jackal;

namespace JackalHost.Actions
{
    public class DropCoins : GameAction
    {
        private readonly Pirate _pirate;

        public DropCoins(Board board, Pirate pirate) : base(board)
        {
            _pirate = pirate;
        }

        public override void Act()
        {
            if (_pirate.Coins == 0)
            {
                return;
            }

            Position position = _pirate.Position;
            Tile tile = Board.Map[position.X, position.Y];
            tile.Coins += _pirate.Coins;
            _pirate.Coins = 0;
        }
    }
}