using System;
using Jackal;

namespace JackalHost
{
    class RandomPlayer : IPlayer
    {
        static readonly Random Rnd = new Random(42);

        public int OnMove(Board board, Move[] availableMoves)
        {
            return Rnd.Next(0, availableMoves.Length);
        }
    }
}