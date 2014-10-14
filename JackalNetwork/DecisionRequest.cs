using System;
using Jackal;

namespace JackalNetwork
{
    public class DecisionRequest : NetworkMessage
    {
        public Guid RequestId;
        public GameState State;
    }
}