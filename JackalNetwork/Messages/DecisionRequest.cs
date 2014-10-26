using System;
using System.Collections.Generic;
using Jackal;

namespace JackalNetwork
{
    public class DecisionRequest : NetworkRequest
    {
        public Guid RequestId;

        public Guid GameId;

        public TileType[,] TileTypes;
        public int[,] TileCoins;

        public Team[] Teams;
        public Move[] AvailableMoves;
        public int TeamId;
        public int TurnNumber;
        public int SubTurnNumber;

        private const int Size = Board.Size;

        public DecisionRequest()
        {
            
        }

        public DecisionRequest(GameState gameState)
        {
            GameId = gameState.GameId;
            Teams = gameState.Board.Teams;
            AvailableMoves = gameState.AvailableMoves;
            TeamId = gameState.TeamId;
            TurnNumber = gameState.TurnNumber;
            SubTurnNumber = gameState.SubTurnNumber;
            TileTypes=new TileType[Size,Size];
            TileCoins=new int[Size,Size];
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    var tile = gameState.Board.Map[x, y];
                    TileTypes[x, y] = tile.Type;
                    TileCoins[x, y] = tile.Coins;
                }
            }
        }

        public GameState GetGameState()
        {
            GameState gameState=new GameState();

            gameState.AvailableMoves = AvailableMoves;
            gameState.GameId = GameId;
            gameState.TurnNumber = TurnNumber;
            gameState.SubTurnNumber = SubTurnNumber;
            gameState.TeamId = TeamId;

            gameState.Board=new Board();
            gameState.Board.Teams = Teams;
            gameState.Board.Map = new Tile[Size, Size];
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    var tile = new Tile();

                    tile.Coins = TileCoins[x, y];
                    tile.Position = new Position(x, y);
                    tile.Type = TileTypes[x, y];
                    tile.Pirates = new HashSet<Pirate>();

                    gameState.Board.Map[x, y] = tile;
                }
            }

            foreach (var team in Teams)
            {
                foreach (var pirate in team.Pirates)
                {
                    var pos = pirate.Position;
                    var tile = gameState.Board.Map[pos.X, pos.Y];
                    if (tile.Type == TileType.Water) continue;
                    tile.Pirates.Add(pirate);
                    tile.OccupationTeamId = pirate.TeamId;
                }
            }

            return gameState;
        }
    }
}