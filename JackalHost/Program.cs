using System;
using Jackal;

namespace JackalHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IPlayer[] players =
            {
                new RandomPlayer(),
                new RandomPlayer(),
                new RandomPlayer(),
                new RandomPlayer(),
            };

            var game = new Game(players);
            var monitor = new Monitor(game);

            while (!game.IsGameOver)
            {
                monitor.Draw();
                
                //Console.ReadKey();
                game.Turn();
            }
            monitor.GameOver();
            Console.ReadKey();
        }
    }
}