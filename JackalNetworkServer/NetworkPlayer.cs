using System;
using Jackal;
using JackalNetwork;

namespace JackalNetworkServer
{
    public class NetworkPlayer : IPlayer
    {
        private readonly ClientInfo _client;
        public NetworkPlayer(ClientInfo client)
        {
            _client = client;
        }

        public void OnNewGame()
        {
            NewGameMessage newGameMessage=new NewGameMessage();
            newGameMessage.GameId = Guid.NewGuid();

            _client.Query(newGameMessage);
        }

        public int OnMove(GameState gameState)
        {
            DecisionRequest request = new DecisionRequest(gameState);
            request.RequestId = Guid.NewGuid();

            DecisionAnswer answer = _client.Query(request) as DecisionAnswer;
            if (answer.RequestId!=request.RequestId) throw new Exception("Wrong requestid");
            return answer.Decision;
        }
    }
}