using System;
using System.Collections.Generic;
using Jackal;

namespace JackalNetwork
{
    public class DecisionRequest : NetworkRequest
    {
        public Guid RequestId;

        public Guid GameId;

        public Map Map;

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
            Map = gameState.Board.Map;
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
            gameState.Board.Map =Map;

            foreach (var team in Teams)
            {
                foreach (var pirate in team.Pirates)
                {
                    var pos = pirate.Position;
                    if (team.Ship.Position == pos.Position)
                    {
                        team.Ship.Crew(gameState.Board).Add(pirate);
                    }
                    else
                    {
                        var tile = gameState.Board.Map[pos.Position];
                        if (tile.Type == TileType.Water) continue;
                        tile.Pirates.Add(pirate);
                    }
                }
            }

            return gameState;
        }
    }
}