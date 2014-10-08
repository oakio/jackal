using Jackal;

namespace JackalHost.Actions
{
    class AttackPirates : GameAction
    {
        private readonly Pirate[] _enemy;

        public AttackPirates(Board board, Pirate[] enemy) : base(board)
        {
            _enemy = enemy;
        }

        public override void Act()
        {
            foreach (var enemy in _enemy)
            {
                Team enemyTeam = Board.Teams[enemy.TeamId];
                Position position = enemy.Position;
                Tile tile = Board.Map[position.X, position.Y];

                tile.Coins += enemy.Coins;
                enemy.Coins = 0;
                enemy.Position = enemyTeam.Ship.Position;
                enemyTeam.Ship.Crew.Add(enemy);
            }
        }
    }
}