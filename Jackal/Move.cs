namespace Jackal
{
    public struct Move
    {
        public Pirate Pirate;

        public Position To;
        public bool WithCoins;

        public Move(Pirate pirate, Position to, bool withCoin)
        {
            Pirate = pirate;
            To = to;
            WithCoins = withCoin;
        }
    }
}