using System;

namespace Jackal.Actions
{
    class TakeCoinToShip : IGameAction
    {
        private readonly Pirate _pirate;
        private readonly Ship _ship;

        public TakeCoinToShip(Pirate pirate,Ship ship)
        {
            _pirate = pirate;
            _ship = ship;
        }

        public GameActionResult Act(Game game)
        {
            Board board = game.Board;
            Position position = _pirate.Position;
            Tile tile = board.Map[position.X, position.Y];
            if (tile.Coins == 0 /* || _pirate.Coins > 0 */)
            {
                throw new Exception("No coins to ship");
            }

            tile.Coins--;
            _ship.Coins++;
            //_pirate.Coins++;

            game.Scores[_pirate.TeamId] ++;
            game.CoinsLeft --;

            game.LastActionTurnNo = game.TurnNo;
            return GameActionResult.Live;
        }
    }
}