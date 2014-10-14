using System;
using Jackal;

namespace JackalHost
{
    class RandomPlayer : IPlayer
    {
        static readonly Random Rnd = new Random(42);

        public int OnMove(Board board, Move[] availableMoves, int teamId)
        {
            return Rnd.Next(0, availableMoves.Length);
        }
    }
}