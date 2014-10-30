﻿using System;
using System.Collections.Generic;
﻿using System.Linq;

namespace Jackal
{
    public class Board
    {
        public const int Size = 13;

        internal MapGenerator Generator;
        public Map Map;
        public Team[] Teams;

        public IEnumerable<Tile> AllTiles(Predicate<Tile> selector)
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                {
                    var tile = Map[i, j];
                    if (selector(tile))
                        yield return tile;
                }
        }

        public Board()
        {
        }

        public Board(int mapId)
        {
            Generator = new MapGenerator(mapId);
            Map = new Map();
            InitMap();

            Teams = new Team[4];
            InitTeam(0, (Size - 1)/2, 0);
            InitTeam(1, 0, (Size - 1) / 2);
            InitTeam(2, (Size - 1) / 2, (Size - 1));
            InitTeam(3, (Size - 1), (Size - 1) / 2);

            Teams[0].Enemies = new[] {1, 2, 3};
            Teams[1].Enemies = new[] {0, 2, 3};
            Teams[2].Enemies = new[] {0, 1, 3};
            Teams[3].Enemies = new[] {0, 1, 2};
        }

        private void InitMap()
        {
            for (int i = 0; i < Size; i++)
            {
                SetWater(i, 0);
                SetWater(0, i);
                SetWater(i, Size - 1);
                SetWater(Size - 1, i);
            }

            for (int x = 1; x < Size - 1; x++)
            {
                for (int y = 1; y < Size - 1; y++)
                {
                    if ((x==1 || x==Size-2) && (y==1||y==Size-2) )
                        SetWater(x, y);
                    else
                        SetUnknown(x, y);
                }
            }
        }

        void SetWater(int x, int y)
        {
            Map[x, y] = new Tile(new Position(x,y),TileType.Water);
        }
        void SetUnknown(int x, int y)
        {
            Map[x, y] = new Tile(new Position(x, y), TileType.Unknown);
        }

        private void InitTeam(int teamId, int x, int y)
        {
            var startPosition = new Position(x, y);
            var pirates = new Pirate[3];
            for (int i = 0; i < pirates.Length; i++)
            {
                pirates[i] = new Pirate(teamId, startPosition);
            }
            var ship = new Ship(teamId, startPosition, new HashSet<Pirate>(pirates));
            Teams[teamId] = new Team(teamId, ship, pirates);
        }

        /// <summary>
        /// Возвращаем список всех полей, в которые можно попасть из исходного поля
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="source"></param>
        /// <param name="alreadyCheckedList"></param>
        /// <returns></returns>
        public List<PossibleMove> GetAllAvaliableMoves(int teamId, Position source, List<CheckedPosition> alreadyCheckedList = null, Direction previosDirection=null)
        {
            if (alreadyCheckedList == null)
                alreadyCheckedList = new List<CheckedPosition>() { };

            var sourceTile = Map[source];

            var ourShip = Teams[teamId].Ship;
            bool fromShip = (ourShip.Position == source);

            List<PossibleMove> goodTargets = new List<PossibleMove>();

            IEnumerable<Position> positionsForCheck;

            if (sourceTile.Type.RequreImmediateMove()) //для клеток с редиректами запоминаем, что в текущую клетку уже не надо возвращаться
            {
                Position previosMoveDelta = null;
                if (sourceTile.Type == TileType.Ice)
                    previosMoveDelta = Position.GetDelta(previosDirection.From, previosDirection.To);
                alreadyCheckedList.Add(new CheckedPosition(source, previosMoveDelta)); //запоминаем, что эту клетку просматривать уже не надо
            }

            //места всех возможных ходов
            positionsForCheck = GetAllTheoreticalMovesForSubturn(source, previosDirection);

            foreach (var newPosition in positionsForCheck)
            {
                if (alreadyCheckedList.Count > 0 && previosDirection!=null)
                {
                    Position incomeDelta = Position.GetDelta(previosDirection.To, newPosition);
                    CheckedPosition currentCheck = new CheckedPosition(newPosition, incomeDelta);

                    if (WasCheckedBefore(alreadyCheckedList, currentCheck)) //мы попали по рекурсии в ранее просмотренную клетку
                    {
                        continue;
                    }
                }

                //проверяем, что на этой клетке
                var newPositionTile = Map[newPosition];

                var newMove = new Move() {From = source, To = newPosition};

                if (sourceTile.Type == TileType.Water && newPositionTile.Type != TileType.Water) //выходим из воды
                {
                    if (source == ourShip.Position && GetShipLanding(ourShip.Position) == newPosition) //только высадка из корабля
                        goodTargets.Add(new PossibleMove(newPosition));
                    continue;
                }

                switch (newPositionTile.Type)
                {
                    case TileType.Water:
                        if (ourShip.Position == newPosition) //заходим на свой корабль
                        {
                            goodTargets.Add(new PossibleMove(newPosition)); //всегда это можем сделать
                        }
                        else if (sourceTile.Type == TileType.Water) //из воды в воду 
                        {
                            if (source != ourShip.Position && GetPosibleSwimming(source).Contains(newPosition)) //пират плавает
                                goodTargets.Add(new PossibleMove(newPosition));
                            if (source == ourShip.Position && GetShipPosibleNavaigations(source).Contains(newPosition)) //корабль плавает
                                goodTargets.Add(new PossibleMove(newPosition));
                        }
                        else //с земли в воду мы можем попасть только если ранее попали на клетку, требующую действия
                        {
                            if (sourceTile.Type.RequreImmediateMove())
                                goodTargets.Add(new PossibleMove(newPosition, PossibleMoveType.JumpToWater));
                        }
                        break;
                    case TileType.RespawnFort:
                    case TileType.Fort:
                        if (newPositionTile.OccupationTeamId.HasValue == false || newPositionTile.OccupationTeamId == teamId) //только если форт не занят
                            goodTargets.Add(new PossibleMove(newPosition));
                        break;
                    case TileType.Grass:
                    case TileType.Chest1:
                    case TileType.Chest2:
                    case TileType.Chest3:
                    case TileType.Chest4:
                    case TileType.Chest5:
                    case TileType.RumBarrel:
                    case TileType.Unknown:
                        goodTargets.Add(new PossibleMove(newPosition));
                        break;
                    case TileType.Horse:
                    case TileType.Arrow:
                    case TileType.Airplane:
                    case TileType.Balloon:
                        goodTargets.AddRange(GetAllAvaliableMoves(teamId, newPosition, alreadyCheckedList, newMove));
                        break;
                }

            }
            return goodTargets;
        }

        /// <summary>
        /// Возвращаем все позиции, в которые в принципе достижимы из заданной клетки за один подход
        /// (не проверяется, допустим ли такой ход)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="previosMove"></param>
        /// <returns></returns>
        public List<Position> GetAllTheoreticalMovesForSubturn(Position source, Direction previosMove)
        {
            var sourceTile = Map[source];

            IEnumerable<Position> rez;
            switch (sourceTile.Type)
            {
                case TileType.Horse:
                    rez = GetHorseDeltas(source);
                    break;
                case TileType.Arrow:
                    rez = GetArrowsDeltas(sourceTile.ArrowsCode, source);
                    break;
                case TileType.Balloon:
                    rez = Teams.Select(x => x.Ship.Position); //на корабль
                    break;
                case TileType.Airplane:
                    if (Map.AirplaneUsed == false)
                    {
                        rez = Teams.Select(x => x.Ship.Position); //на корабль
                        var airplaneTargets = AllTiles(x => x.Type != TileType.Water
                                                            && x.Type.RequreImmediateMove() == false
                                                            && x.Type != TileType.Airplane)
                            .Select(x => x.Position);
                        rez = rez.Concat(airplaneTargets);
                    }
                    else
                    {
                        rez = GetNearDeltas(source);
                    }
                    break;
                case TileType.Croc:
                    rez = new[] {previosMove.From}; //возвращаемся назад
                    break;
                case TileType.Ice:
                    if (Map[previosMove.From].Type == TileType.Horse)
                    {
                        //повторяем ход лошади
                        rez = GetHorseDeltas(source);
                    }
                    else //повторяем предыдущий ход
                    {
                        //TODO - проверка на использование самолета на предыдущем ходу - тогда мы должны повторить ход самолета
                        var previosDelta = Position.GetDelta(previosMove.From, previosMove.To);
                        Position target = Position.AddDelta(source, previosDelta);
                        rez = new[] {target};
                    }
                    break;
                case TileType.RespawnFort:
                    rez= GetNearDeltas(source).Concat(new[]{source});
                    break;
                default:
                    rez = GetNearDeltas(source);
                    break;
            }
            return rez.Where(x => IsValidMapPosition(x)).ToList();
        }

        public IEnumerable<Position> GetHorseDeltas(Position pos)
        {
            for (int x = -2; x <= 2; x++)
            {
                if (x == 0) continue;
                int deltaY = (Math.Abs(x) == 2) ? 1 : 2;
                yield return new Position(pos.X + x, pos.Y - deltaY);
                yield return new Position(pos.X + x, pos.Y + deltaY);
            }
        }

        public IEnumerable<Position> GetNearDeltas(Position pos)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    yield return new Position(pos.X + x, pos.Y + y);
                }
            }
        }

        public bool IsValidMapPosition(Position pos)
        {
            return (
                pos.X >= 0 && pos.X < Board.Size
                && pos.Y >= 0 && pos.Y < Board.Size //попадаем в карту
                && (pos.X != 0 || pos.Y != 0) //не попадаем в углы карты
                && (pos.X != 0 || pos.Y != Board.Size - 1)
                && (pos.X != Board.Size - 1 || pos.Y != 0)
                && (pos.X != Board.Size - 1 || pos.Y != Board.Size - 1)
                );
        }

        public IEnumerable<Position> GetShipPosibleNavaigations(Position pos)
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

        public Position GetShipLanding(Position pos)
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

        public IEnumerable<Position> GetArrowsDeltas(int arrowsCode, Position source)
        {
            foreach (var delta in ArrowsCodesHelper.GetExitDeltas(arrowsCode))
            {
                yield return new Position(source.X + delta.X, source.Y + delta.Y);
            }
        }

        /// <summary>
        /// Все возможные цели для плавающего пирата
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public IEnumerable<Position> GetPosibleSwimming(Position pos)
        {
            return GetNearDeltas(pos).Where(x => IsValidMapPosition(x)).Where(x => Map[x].Type == TileType.Water);
        }

        public bool WasCheckedBefore(List<CheckedPosition> alreadyCheckedList, CheckedPosition currentCheck)
        {
            foreach (var info in alreadyCheckedList)
            {
                if (info.Position == currentCheck.Position)
                {
                    if (info.IncomeDelta == null || info.IncomeDelta == currentCheck.IncomeDelta) return true;
                }
            }
            return false;
        }
    }
}