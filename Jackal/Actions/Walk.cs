using System;

namespace Jackal.Actions
{
    class Walk : IGameAction
    {
        private readonly Pirate _pirate;
        private readonly Position _to;
        private bool _withCoin;

        public Walk(Pirate pirate, Position to, bool withCoin = false)
        {
            _pirate = pirate;
            _to = to;
            _withCoin = withCoin;
        }

        public GameActionResult Act(Game game)
        {
            var map = game.Board.Map;

            var from = _pirate.Position;

            var fromTile = map[from.X, from.Y];
            fromTile.Pirates.Remove(_pirate);
            if (fromTile.Pirates.Count == 0)
            {
                fromTile.OccupationTeamId = null;
            }

            _pirate.Position = _to;
            var toTile = map[_to.X, _to.Y];
            toTile.OccupationTeamId = _pirate.TeamId;
            toTile.Pirates.Add(_pirate);

            if (_withCoin)
            {
                if (fromTile.Coins == 0) throw new Exception("No coins");
                fromTile.Coins--;
                toTile.Coins++;
            }

            if (toTile.Type == TileType.RumBarrel)
            {
                _pirate.DrunkSinceTurnNo = game.TurnNo;
                _pirate.IsDrunk = true;
            }

            return GameActionResult.Live;
        }
    }
}