using Jackal;

namespace JackalHost.Actions
{
    class AttackPirates : IGameAction
    {
        private readonly Pirate[] _enemy;

        public AttackPirates(Pirate[] enemy)
        {
            _enemy = enemy;
        }

        public void Act(Game game)
        {
            Board board = game.Board;
            foreach (var enemy in _enemy)
            {
                Team enemyTeam = board.Teams[enemy.TeamId];
                Position position = enemy.Position;
                Tile tile = board.Map[position.X, position.Y];

                tile.Coins += enemy.Coins;
                enemy.Coins = 0;
                enemy.Position = enemyTeam.Ship.Position;
                enemyTeam.Ship.Crew.Add(enemy);
            }
        }
    }
}