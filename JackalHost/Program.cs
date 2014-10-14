using System;
using System.Threading;
using Jackal;

namespace JackalHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IPlayer[] players =
            {
                new SmartPlayer(),
                new SmartPlayer(),
                new SmartPlayer(),
                new SmartPlayer(),
            };
            const int mapId = 987412;

            var board = new Board(mapId);
            var game = new Game(players, board);
            var monitor = new Monitor(game);

            while (!game.IsGameOver)
            {
                //if (game.TurnNo%100 == 0)
                {
                    monitor.Draw();
                }
                Thread.Sleep(TimeSpan.FromMilliseconds(1));
                //Console.ReadKey();
                game.Turn();
            }
            monitor.GameOver();
            Console.ReadKey();
        }
    }
}