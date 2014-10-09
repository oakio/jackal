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
            const int mapId = 987412;

            var mapGen = new MapGenerator(mapId);
            var game = new Game(players, mapGen);
            var monitor = new Monitor(game);

            while (!game.IsGameOver)
            {

                if (game.TurnNo%100 == 0)
                {
                    monitor.Draw();
                }
                //Console.ReadKey();
                game.Turn();
            }
            monitor.GameOver();
            Console.ReadKey();
        }
    }
}