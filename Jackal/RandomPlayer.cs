using System;

namespace Jackal
{
    public class RandomPlayer : IPlayer
    {
        static readonly Random Rnd = new Random(42);

        public int OnMove(GameState gameState)
        {
            return Rnd.Next(gameState.AvailableMoves.Length);
        }
    }
}