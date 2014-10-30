using System.Linq;

namespace Jackal.Actions
{
    class Respawn : IGameAction
    {
        private readonly Position _to;
        private readonly Team _team;

        public Respawn(Team team, Position to)
        {
            _to = to;
            _team = team;
        }

        public GameActionResult Act(Game game)
        {
            Board board = game.Board;
            Tile tile = board.Map[_to];

            Pirate pirate = new Pirate(_team.Id, _to);
            _team.Pirates = _team.Pirates.Concat(new[] {pirate}).ToArray();
            tile.Pirates.Add(pirate);
            
            return GameActionResult.Live;
        }
    }
}