using System;

namespace JackalNetwork
{
    public class DecisionAnswer : NetworkMessage
    {
        public Guid RequestId;
        public int Decision;
    }
}