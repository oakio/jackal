using System;
using System.Collections.Generic;
using Jackal.Actions;

namespace Jackal
{
    public class Game
    {
        private readonly IPlayer[] _players;

        public Board Board;

        public Dictionary<int, int> Scores; // TeamId->Total couns
        public int CoinsLeft;

        private readonly List<Move> _availableMoves;
        private readonly List<IGameAction> _actions;

        public readonly Guid GameId = Guid.NewGuid();

        public Game(IPlayer[] players, Board board)
        {
            _players = players;

            Board = board;
            Scores = new Dictionary<int, int>();
            foreach (var team in Board.Teams)
            {
                Scores[team.Id] = 0;
            }
            CoinsLeft = Board.Generator.TotalCoins;

            _availableMoves = new List<Move>();
            _actions = new List<IGameAction>();
        }

        public bool Turn()
        {
            int teamId = CurrentTeamId;
            IPlayer me = _players[teamId];

            GetAvailableMoves(teamId);

            GameState gameState=new GameState();
            gameState.AvailableMoves = _availableMoves.ToArray();
            gameState.Board = Board;
            gameState.GameId = GameId;
            gameState.TurnNumber = TurnNo;
            gameState.SubTurnNumber = 1;
            gameState.TeamId = teamId;
            int moveNo = me.OnMove(gameState);

            IGameAction action = _actions[moveNo];
            action.Act(this);
            TurnNo++;
            return true;
        }

        private void GetAvailableMoves(int teamId)
        {
            _availableMoves.Clear();
            _actions.Clear();
            
            Team team = Board.Teams[teamId];
            Ship ship = team.Ship;

            foreach (var pirate in team.Pirates)
            {
                Position position = pirate.Position;

                if (position.Y > 0) // N
                {
                    Step(position.X, position.Y - 1, pirate, ship, team);
                }
                if (position.X < (Board.Size-1) && position.Y > 0) // NE
                {
                    Step(position.X + 1, position.Y - 1, pirate, ship, team);
                }
                if (position.X < (Board.Size - 1)) // E
                {
                    Step(position.X + 1, position.Y, pirate, ship, team);
                }
                if (position.X < (Board.Size - 1) && position.Y < (Board.Size-1)) // SE
                {
                    Step(position.X + 1, position.Y + 1, pirate, ship, team);
                }
                if (position.Y < (Board.Size - 1)) // S
                {
                    Step(position.X, position.Y + 1, pirate, ship, team);
                }
                if (position.X > 0 && position.Y < (Board.Size - 1)) // SW
                {
                    Step(position.X - 1, position.Y + 1, pirate, ship, team);
                }
                if (position.X > 0) // W
                {
                    Step(position.X - 1, position.Y, pirate, ship, team);
                }
                if (position.X > 0 && position.Y > 0) // NW
                {
                    Step(position.X - 1, position.Y - 1, pirate, ship, team);
                }
            }
        }

        private void AddMoveAndActions(Move move, IGameAction action)
        {
            if (_availableMoves.Exists(x => x == move)) return;
            _availableMoves.Add(move);
            _actions.Add(action);
        }

        private void Step(int toX, int toY, Pirate pirate, Ship ship, Team team)
        {
            //var moves = _availableMoves;
            //var actions = _actions;

            var target = new Position(toX, toY);
            Tile targetTile = Board.Map[toX, toY];

            var source = pirate.Position;
            Tile sourceTile = Board.Map[source.X, source.Y];

            bool onShip = (ship.Position == pirate.Position);

            switch (targetTile.Type)
            {
                case TileType.Unknown:
                {
                    // exploration

                    if (onShip)
                    {
                        if (CanLanding(pirate, target))
                        {
                            // landing
                            AddMoveAndActions(new Move(pirate, target, false),
                                GameActionList.Create(
                                    //new DropCoin(pirate),
                                    new Explore(target),
                                    new Landing(pirate, ship)));
                        }
                    }
                    else
                    {
                        AddMoveAndActions(new Move(pirate, target, false),
                            GameActionList.Create(
                                //new DropCoin(pirate),
                                new Explore(target),
                                new Walk(pirate, target)));
                    }

                    break;
                }
                case TileType.Stone:
                {
                    return;
                }
                case TileType.Water:
                {
                    if (target == ship.Position)
                    {
                        if (sourceTile.Coins > 0)
                        {
                            // shipping with coins
                            AddMoveAndActions(new Move(pirate, target, true),
                                GameActionList.Create(
                                    new TakeCoinToShip(pirate,ship),
                                    new Shipping(pirate, ship)));
                        }

                        // shipping without coins
                        AddMoveAndActions(new Move(pirate, target, false),
                            GameActionList.Create(
                                //new DropCoin(pirate),
                                new Shipping(pirate, ship)));
                    }
                    else if (pirate.Position == ship.Position)
                    {
                        if (((ship.Position.X == 0 || ship.Position.X == Board.Size - 1) &&
                             (target.Y <= 1 || target.Y >= Board.Size - 2)) 
                             ||
                            ((ship.Position.Y == 0 || ship.Position.Y == Board.Size - 1) &&
                             (target.X <= 1 || target.X >= Board.Size - 2)))
                        {
                            break; // enemy water territories
                        }

                        // navigation
                        AddMoveAndActions(new Move(pirate, target, false),
                            GameActionList.Create(
                                new Navigation(ship, target)));
                    }
                    break;
                }
                case TileType.Grass:
                {
                    var attack = targetTile.OccupationTeamId.HasValue && targetTile.OccupationTeamId.Value != pirate.TeamId;
                    if (attack)
                    {
                        // attack
                        if (onShip)
                        {
                            if (CanLanding(pirate, target))
                            {
                                AddMoveAndActions(new Move(pirate, target, false),
                                    GameActionList.Create(
                                        //new DropCoin(pirate),
                                        new Attack(target),
                                        new Landing(pirate, ship)));
                            }
                        }
                        else
                        {
                            AddMoveAndActions(new Move(pirate, target, false),
                                GameActionList.Create(
                                    //new DropCoin(pirate),
                                    new Attack(target),
                                    new Walk(pirate, target)));
                        }
                    }
                    else
                    {
                        if (onShip)
                        {
                            if (CanLanding(pirate, target))
                            {
                                AddMoveAndActions(new Move(pirate, target, false),
                                    GameActionList.Create(
                                        new Landing(pirate, ship)));
                            }
                        }
                        else
                        {
                            AddMoveAndActions(new Move(pirate, target, false),
                                GameActionList.Create(
                                    //new DropCoin(pirate),
                                    new Walk(pirate, target)));

                            if (sourceTile.Coins > 0)
                            {
                                AddMoveAndActions(new Move(pirate, target, true),
                                    GameActionList.Create(
                                        //new MoveCoin(pirate,to),
                                        new Walk(pirate, target,true)));
                            }
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
            get { return CoinsLeft == 0; }
        }

        public int TurnNo { get; set; }

        public int CurrentTeamId
        {
            get { return TurnNo%4; }
        }
    }
}
