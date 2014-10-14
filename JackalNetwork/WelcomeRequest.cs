using System;

namespace JackalNetwork
{
    public class WelcomeRequest : NetworkMessage
    {
        public Guid ServerId;
        public int DecisionTimeout;
    }
}