using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using Jackal;
using JackalHost.Actions;

namespace JackalHost
{
    public class Game
    {
        private readonly IPlayer[] _players;

        public Board Board;
        public Dictionary<int, int> Scores; // TeamId->Total couns

        public Game(IPlayer[] players, Board board)
        {
            _players = players;

            Board = board;
            Scores = new Dictionary<int, int>();
        }

        public bool Turn()
        {
            int teamId = CurrentTeamId;
            IPlayer me = _players[teamId];

            List<Move> availableMoves;
            List<IGameAction> actions;
            GetAvailableMoves(teamId, out availableMoves, out actions);

            int moveNo = me.OnMove(Board, availableMoves.ToArray());

            IGameAction action = actions[moveNo];
            action.Act(this);
            TurnNo++;
            return true;
        }

        private void GetAvailableMoves(int teamId, out List<Move> moves, out List<IGameAction> actions)
        {
            Team team = Board.Teams[teamId];
            Ship ship = team.Ship;

            moves = new List<Move>();
            actions = new List<IGameAction>();

            foreach (var pirate in team.Pirates)
            {
                Position position = pirate.Position;

                if (position.Y > 0) // N
                {
                    Step(moves, actions, position.X, position.Y - 1, pirate, ship, team);
                }
                if (position.X < (Board.Size-1) && position.Y > 0) // NE
                {
                    Step(moves, actions, position.X + 1, position.Y - 1, pirate, ship, team);
                }
                if (position.X < (Board.Size - 1)) // E
                {
                    Step(moves, actions, position.X + 1, position.Y, pirate, ship, team);
                }
                if (position.X < (Board.Size - 1) && position.Y < (Board.Size-1)) // SE
                {
                    Step(moves, actions, position.X + 1, position.Y + 1, pirate, ship, team);
                }
                if (position.Y < (Board.Size - 1)) // S
                {
                    Step(moves, actions, position.X, position.Y + 1, pirate, ship, team);
                }
                if (position.X > 0 && position.Y < (Board.Size - 1)) // SW
                {
                    Step(moves, actions, position.X - 1, position.Y + 1, pirate, ship, team);
                }
                if (position.X > 0) // W
                {
                    Step(moves, actions, position.X - 1, position.Y, pirate, ship, team);
                }
                if (position.X > 0 && position.Y > 0) // NW
                {
                    Step(moves, actions, position.X - 1, position.Y - 1, pirate, ship, team);
                }

            }
        }

        private void Step(List<Move> moves, List<IGameAction> actions, int toX, int toY, Pirate pirate, Ship ship, Team team)
        {
            var to = new Position(toX, toY);
            Tile tile = Board.Map[toX, toY];

            bool onShip = (ship.Position == pirate.Position);

            switch (tile.Type)
            {
                case TileType.Unknown:
                {
                    // exploration
                    
                    if (onShip)
                    {
                        if (CanLanding(pirate, to))
                        {
                            // landing
                            moves.Add(new Move(pirate, to, false));
                            actions.Add(GameActionList.Create(
                                new DropCoin(pirate),
                                new Explore(to),
                                new Landing(pirate, ship)));
                        }
                    }
                    else
                    {
                        moves.Add(new Move(pirate, to, false));
                        actions.Add(GameActionList.Create(
                            new DropCoin(pirate),
                            new Explore(to),
                            new Walk(pirate, to)));
                    }

                    break;
                }
                case TileType.Stone:
                {
                    return;
                }
                case TileType.Water:
                {
                    if (to == ship.Position)
                    {
                        // shipping with coins
                        moves.Add(new Move(pirate, to, true));
                        actions.Add(GameActionList.Create(
                            new TakeCoin(pirate),
                            new Shipping(pirate, ship)));

                        // shipping without coins
                        moves.Add(new Move(pirate, to, false));
                        actions.Add(GameActionList.Create(
                            new DropCoin(pirate),
                            new Shipping(pirate, ship)));
                    }
                    else if (pirate.Position == ship.Position)
                    {
                        if (((ship.Position.X == 0 || ship.Position.X == Board.Size - 1) &&
                             (to.Y == 0 || to.Y == Board.Size - 1)) ||
                            ((ship.Position.Y == 0 || ship.Position.Y == Board.Size - 1) &&
                             (to.X == 0 || to.X == Board.Size - 1)))
                        {
                            break; // enemy water territories
                        }

                        // navigation
                        moves.Add(new Move(pirate, to, true));
                        actions.Add(GameActionList.Create(
                            new Naviation(ship, to)));
                    }
                    break;
                }
                case TileType.Grass:
                {
                    var attack = tile.OccupationTeamId.HasValue && tile.OccupationTeamId.Value != pirate.TeamId;
                    if (attack)
                    {
                        // attack
                        if (onShip)
                        {
                            if (CanLanding(pirate, to))
                            {
                                moves.Add(new Move(pirate, to, false));
                                actions.Add(GameActionList.Create(
                                    new DropCoin(pirate),
                                    new Attack(to),
                                    new Landing(pirate, ship)));
                            }
                        }
                        else
                        {
                            moves.Add(new Move(pirate, to, false));
                            actions.Add(GameActionList.Create(
                                new DropCoin(pirate),
                                new Attack(to),
                                new Walk(pirate, to)));
                        }
                    }
                    else
                    {
                        if (onShip)
                        {
                            if (CanLanding(pirate, to))
                            {
                                moves.Add(new Move(pirate, to, false));
                                actions.Add(GameActionList.Create(
                                    new Landing(pirate, ship)));
                            }
                        }
                        else
                        {
                            moves.Add(new Move(pirate, to, false));
                            actions.Add(GameActionList.Create(
                                new DropCoin(pirate),
                                new Walk(pirate, to)));

                            moves.Add(new Move(pirate, to, true));
                            actions.Add(GameActionList.Create(
                                new TakeCoin(pirate),
                                new Walk(pirate, to)));
                        }                       
                    }
                    break;
                }
            }
        }

        private static bool CanLanding(Pirate pirate, Position to)
        {
            return ((pirate.Position.Y == 0 || pirate.Position.Y == Board.Size - 1) &&
                    pirate.Position.X - to.X == 0) ||
                   ((pirate.Position.X == 0 || pirate.Position.X == Board.Size - 1) &&
                    pirate.Position.Y - to.Y == 0);
        }

        public bool IsGameOver
        {
            get
            {
                int totalCoins = 0;
                foreach (var team in Board.Teams)
                {
                    var coins = team.Ship.Coins;
                    totalCoins += coins;
                    Scores[team.Id] = coins;
                }
                return totalCoins == Board.Generator.TotalCoins;
            }
        }

        public int TurnNo { get; set; }

        public int CurrentTeamId
        {
            get { return TurnNo%4; }
        }
    }
}
