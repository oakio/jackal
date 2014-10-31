namespace Jackal.Actions
{
    class Attack : IGameAction
    {
        private readonly Position _to;

        public Attack(Position to)
        {
            _to = to;
        }

        public GameActionResult Act(Game game)
        {
            Board board = game.Board;
            Tile tile = board.Map[_to];

            foreach (var enemyPirate in tile.Pirates)
            {
                Team enemyTeam = board.Teams[enemyPirate.TeamId];
                //tile.Coins += enemyPirate.Coins;
                //enemyPirate.Coins = 0;
                enemyPirate.Position = new TilePosition(enemyTeam.Ship.Position);
                enemyTeam.Ship.Crew(game.Board).Add(enemyPirate);
            }
            tile.Pirates.Clear();

            return GameActionResult.Live;
        }
    }
}