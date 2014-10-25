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
        }

        public int OnMove(GameState gameState)
        {
            DecisionRequest request = new DecisionRequest();
            request.RequestId = Guid.NewGuid();
            request.State = gameState;

            DecisionAnswer answer = _client.Query(request) as DecisionAnswer;
            if (answer.RequestId!=request.RequestId) throw new Exception("Wrong requestid");
            return answer.Decision;
        }
    }
}