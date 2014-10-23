namespace Jackal.Actions
{
    class Attack : IGameAction
    {
        private readonly Position _to;

        public Attack(Position to)
        {
            _to = to;
        }
        
        public void Act(Game game)
        {
            Board board = game.Board;
            Tile tile = board.Map[_to.X, _to.Y];

            foreach (var enemyPirate in tile.Pirates)
            {
                Team enemyTeam = board.Teams[enemyPirate.TeamId];
                //tile.Coins += enemyPirate.Coins;
                //enemyPirate.Coins = 0;
                enemyPirate.Position = enemyTeam.Ship.Position;
                enemyTeam.Ship.Crew.Add(enemyPirate);
            }
            tile.OccupationTeamId = null;
            tile.Pirates.Clear();
        }
    }
}