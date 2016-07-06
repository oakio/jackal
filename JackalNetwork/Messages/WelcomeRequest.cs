using System;

namespace JackalNetwork
{
    public class WelcomeRequest : NetworkRequest
    {
        public Guid ServerId;
        public int DecisionTimeout;
    }
}