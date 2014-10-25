using System;

namespace Jackal
{
    public class RandomPlayer : IPlayer
    {
        private Random Rnd;

        public void OnNewGame()
        {
            Rnd = new Random(42);
        }

        public int OnMove(GameState gameState)
        {
            return Rnd.Next(gameState.AvailableMoves.Length);
        }
    }
}