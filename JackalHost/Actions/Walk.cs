using Jackal;

namespace JackalHost.Actions
{
    class Walk : IGameAction
    {
        private readonly Pirate _pirate;
        private readonly Position _to;

        public Walk(Pirate pirate, Position to)
        {
            _pirate = pirate;
            _to = to;
        }

        public void Act(Game game)
        {
            Tile[,] map = game.Board.Map;

            var from = _pirate.Position;

            var fromTile = map[@from.X, @from.Y];
            fromTile.Pirates.Remove(_pirate);
            if (fromTile.Pirates.Count == 0)
            {
                fromTile.OccupationTeamId = null;
            }

            _pirate.Position = _to;
            var toTile = map[_to.X, _to.Y];
            toTile.OccupationTeamId = _pirate.TeamId;
            toTile.Pirates.Add(_pirate);
        }
    }
}