using System;
using System.Collections.Generic;
using System.Linq;
using Jackal.Actions;

namespace Jackal
{
    public class Game
    {
        private readonly IPlayer[] _players;

        public readonly Board Board;

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
            CoinsLeft = MapGenerator.TotalCoins;

            _availableMoves = new List<Move>();
            _actions = new List<IGameAction>();

            foreach (var player in _players)
            {
                player.OnNewGame();
            }
        }

        public Move Turn()
        {
            int teamId = CurrentTeamId;
            IPlayer me = _players[teamId];

            GetAvailableMoves(teamId);

            this.NeedSubTurnPirate = null;
            Move selectedMove = null;

            if (_availableMoves.Count > 0) //есть возможные ходы
            {
                int moveNo;
                if (_availableMoves.Count == 1) //только один ход, сразу выбираем его
                {
                    moveNo = 0;
                }
                else //запрашиваем ход у игрока
                {
                    GameState gameState = new GameState();
                    gameState.AvailableMoves = _availableMoves.ToArray();
                    gameState.Board = Board;
                    gameState.GameId = GameId;
                    gameState.TurnNumber = TurnNo;
                    gameState.SubTurnNumber = SubTurnNo;
                    gameState.TeamId = teamId;
                    moveNo = me.OnMove(gameState);
                }

                IGameAction action = _actions[moveNo];
                selectedMove= _availableMoves[moveNo];
                action.Act(this);
            }

            if (this.NeedSubTurnPirate == null)
            {
                //также протрезвляем всех пиратов, которые начали бухать раньше текущего хода
                foreach (Pirate pirate in Board.Teams[teamId].Pirates.Where(x=>x.IsDrunk && x.DrunkSinceTurnNo<TurnNo))
                {
                    pirate.DrunkSinceTurnNo = null;
                    pirate.IsDrunk = false;
                }

                TurnNo++;
                SubTurnNo = 0;
            }
            else
            {
                SubTurnNo++;
            }

            return selectedMove;
        }

        public Pirate NeedSubTurnPirate { private get; set; }

        private void GetAvailableMoves(int teamId)
        {
            _availableMoves.Clear();
            _actions.Clear();
            
            Team team = Board.Teams[teamId];
            Ship ship = team.Ship;


            IEnumerable<Pirate> activePirates;
            if (NeedSubTurnPirate != null)
                activePirates = new[] {NeedSubTurnPirate};
            else
                activePirates = team.Pirates.Where(x => x.IsDrunk == false);

            foreach (var pirate in activePirates)
            {
                Position position = pirate.Position;

                var targets = GetAllAvaliableMoves(teamId,position);
                foreach (Position target in targets)
                {
                    Step(target.X, target.Y, pirate, ship, team);
                }
                /*
                if (position.Y > 0) // N
                {
                    Step(position.X, position.Y - 1, pirate, ship, team);
                }
                if (position.X < (Board.Size - 1) && position.Y > 0) // NE
                {
                    Step(position.X + 1, position.Y - 1, pirate, ship, team);
                }
                if (position.X < (Board.Size - 1)) // E
                {
                    Step(position.X + 1, position.Y, pirate, ship, team);
                }
                if (position.X < (Board.Size - 1) && position.Y < (Board.Size - 1)) // SE
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
                */ 
            }
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
                                    new Explore(target,pirate),
                                    new Landing(pirate, ship),
                                    new Walk(pirate, target)));
                        }
                    }
                    else
                    {
                        AddMoveAndActions(new Move(pirate, target, false),
                            GameActionList.Create(
                                //new DropCoin(pirate),
                                new Explore(target, pirate),
                                new Walk(pirate, target)));
                    }

                    break;
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
				case TileType.Chest1:
                case TileType.Chest2:
                case TileType.Chest3:
                case TileType.Chest4:
                case TileType.Chest5:
                case TileType.Grass:
                case TileType.Fort:
                case TileType.RespawnFort:
                case TileType.RumBarrel:
                {
                    var attack = targetTile.OccupationTeamId.HasValue && targetTile.OccupationTeamId.Value != pirate.TeamId;
                    bool targetIsFort = targetTile.Type.IsFort();

                    bool targetIsRumbar = targetTile.Type == TileType.RumBarrel;
                    

                    if (attack)
                    {
                        if (targetIsFort == false)
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
                                            new Landing(pirate, ship),
                                            new Walk(pirate, target)));
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
                    }
                    else
                    {
                        if (onShip)
                        {
                            if (CanLanding(pirate, target))
                            {
                                AddMoveAndActions(new Move(pirate, target, false),
                                    GameActionList.Create(
                                        new Landing(pirate, ship),
                                        new Walk(pirate, target)));
                            }
                        }
                        else
                        {
                            AddMoveAndActions(new Move(pirate, target, false),
                                GameActionList.Create(
                                    //new DropCoin(pirate),
                                    new Walk(pirate, target)));

                            if (sourceTile.Coins > 0 && targetIsFort == false)
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

        private void AddMoveAndActions(Move move, IGameAction action)
        {
            if (_availableMoves.Exists(x => x == move)) return;
            _availableMoves.Add(move);
            _actions.Add(action);
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
            get { return CoinsLeft == 0 || TurnNo - 4*50 > LastActionTurnNo; }
        }

        public int TurnNo { get; private set; }
        public int SubTurnNo { get; private set; }
        public int LastActionTurnNo { get; internal set; }

        public int CurrentTeamId
        {
            get { return TurnNo%4; }
        }

        public void KillPirate(Pirate pirate)
        {
            int teamId = pirate.TeamId;
            Board.Teams[teamId].Pirates = Board.Teams[teamId].Pirates.Where(x => x != pirate).ToArray();
            Board.Teams[teamId].Ship.Crew.Remove(pirate);
            var tile = Board.Map[pirate.Position.X, pirate.Position.Y];
            tile.Pirates.Remove(pirate);
        }

        /// <summary>
        /// Возвращаем список всех полей, в которые можно попасть из исходной
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="source"></param>
        /// <param name="alreadyCheckedList"></param>
        /// <returns></returns>
        public List<Position> GetAllAvaliableMoves(int teamId,Position source,List<Position> alreadyCheckedList=null)
        {
            if (alreadyCheckedList == null)
                alreadyCheckedList = new List<Position>() {};

            var sourceTile = Board.Map[source.X, source.Y];

            var ourShip = Board.Teams[teamId].Ship;
            bool fromShip = (ourShip.Position == source);

            List<Position> goodTargets=new List<Position>();

            IEnumerable<Position> positionsForCheck;
            switch (sourceTile.Type)
            {
                case TileType.Horse:
                    alreadyCheckedList.Add(source);
                    positionsForCheck = GetHorseDeltas(source);
                    break;
                case TileType.Arrow:
                    alreadyCheckedList.Add(source);
                    positionsForCheck = GetArrowsDeltas(sourceTile.ArrowsCode, source);
                    break;
                case TileType.Water:
                    goodTargets.AddRange(GetShipPosibleNavaigations(source)); //мы всегда можем сдвинуть свой корабль
                    positionsForCheck = new[] {GetShipLanding(source)}; //или попробовать высадиться
                    break;
                default:
                    positionsForCheck = GetNearDeltas(source);
                    break;
            }
            foreach (var newPosition in positionsForCheck)
            {
                if (IsMapPosition(newPosition))
                {
                    if (alreadyCheckedList.Contains(newPosition)) //мы попали по рекурсии в ранее просмотренную клетку
                    {
                        continue;
                    }

                    //проверяем, что на этой клетке
                    var newPositionTile = Board.Map[newPosition.X, newPosition.Y];
                    switch (newPositionTile.Type)
                    {
                        case TileType.Water:
                            if (ourShip.Position == newPosition) //заходим на свой корабль
                                goodTargets.Add(newPosition);
                            break;
                        case TileType.RespawnFort:
                        case TileType.Fort:
                            if (newPositionTile.OccupationTeamId.HasValue == false || newPositionTile.OccupationTeamId == teamId)
                                goodTargets.Add(newPosition);
                            break;
                        case TileType.Grass:
                        case TileType.Chest1:
                        case TileType.Chest2:
                        case TileType.Chest3:
                        case TileType.Chest4:
                        case TileType.Chest5:
                        case TileType.RumBarrel:
                        case TileType.Unknown:
                            goodTargets.Add(newPosition);
                            break;
                        case TileType.Horse:
                            goodTargets.AddRange(GetAllAvaliableMoves(teamId, newPosition, alreadyCheckedList));
                            break;
                        case TileType.Arrow:
                            goodTargets.AddRange(GetAllAvaliableMoves(teamId, newPosition, alreadyCheckedList));
                            break;
                    }
                }
            }
            return goodTargets;
        }

        private IEnumerable<Position> GetArrowsDeltas(int arrowsCode, Position source)
        {
            foreach (var delta in ArrowsCodesHelper.GetExitDeltas(arrowsCode))
            {
                yield return new Position(source.X + delta.X, source.Y + delta.Y);
            }
        }

        private Position GetShipLanding(Position pos)
        {
            if (pos.X == 0)
            {
                 return new Position(1, pos.Y );
            }
            else if (pos.X == 12)
            {
                return new Position(11, pos.Y);
            }
            else if (pos.Y == 0)
            {
                return new Position(pos.X,1);
            }
            else if (pos.Y == 12)
            {
                return new Position(pos.X, 11);
            }
            else
            {
                throw new Exception("wrong ship position");
            }
        }

        private IEnumerable<Position> GetShipPosibleNavaigations(Position pos)
        {
            if (pos.X == 0 || pos.X==12)
            {
                if (pos.Y>2)
                    yield return new Position(pos.X,pos.Y-1);
                if (pos.Y<10)
                    yield return new Position(pos.X,pos.Y+1);
            }
            else if (pos.Y == 0 || pos.Y==12)
            {
                if (pos.X>2)
                    yield return new Position(pos.X-1,pos.Y);
                if (pos.X<10)
                    yield return new Position(pos.X+1,pos.Y);
            }
            else
            {
                throw new Exception("wrong ship position");
            }
        }

        private IEnumerable<Position> GetShipDeltas(Position source)
        {
            throw new NotImplementedException();
        }

        private bool IsMapPosition(Position pos)
        {
            return (pos.X >= 0 && pos.X < Board.Size
                    && pos.Y >= 0 && pos.Y < Board.Size);
        }

        private IEnumerable<Position> GetNearDeltas(Position pos)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    yield return new Position(pos.X+x,pos.Y+y);
                }
            }
        }

        private IEnumerable<Position> GetHorseDeltas(Position pos)
        {
            for (int x = -2; x <= 2; x++)
            {
                if (x == 0) continue;
                int deltaY = (Math.Abs(x) == 2) ? 1 : 2;
                yield return new Position(pos.X + x, pos.Y - deltaY);
                yield return new Position(pos.X + x, pos.Y + deltaY);
            }
        }
    }
}
