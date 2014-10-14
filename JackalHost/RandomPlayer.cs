using System;
using Jackal;

namespace JackalHost
{
    class RandomPlayer : IPlayer
    {
        static readonly Random Rnd = new Random(42);

        public int OnMove(GameState gameState)
        {
            return Rnd.Next(gameState.AvailableMoves.Length);
        }
    }
}