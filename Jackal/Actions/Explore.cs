namespace Jackal.Actions
{
    class Explore : IGameAction
    {
        private readonly Position _position;
        private readonly Pirate _pirate;

        public Explore(Position position, Pirate pirate)
        {
            _position = position;
            _pirate = pirate;
        }

        public GameActionResult Act(Game game)
        {
            Board board = game.Board;
            var newTile = board.Generator.GetNext(_position);
            newTile.Position = new Position(_position.X, _position.Y);
            board.Map[_position.X, _position.Y] = newTile;

            game.LastActionTurnNo = game.TurnNo;

            if (newTile.Type == TileType.Horse)
            {
                var targets = game.GetAllAvaliableMoves(_pirate.TeamId,_position);
                if (targets.Count == 0)
                {
                    game.KillPirate(_pirate);
                    return GameActionResult.Die;
                }
                else //мы попали в клетку, где должны сделать ещё свой выбор
                {
                    game.NeedSubTurnPirate = _pirate;
                }
            }
            return GameActionResult.Live;
        }
    }
}