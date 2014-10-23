using System;
using System.Threading;
using System.Windows.Forms;
using Jackal;
using JackalHost.Monitors;

namespace JackalHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IPlayer[] players =
            {
                new RandomPlayer(), 
                new SmartPlayer(),
                new SmartPlayer(),
                new SmartPlayer(),
            };
            const int mapId = 987412+1;

            var board = new Board(mapId);
            var game = new Game(players, board);
            //var monitor = new JackalHost.Monitors.WinFormMonitor(game);


            Application.EnableVisualStyles();
            Application.Run(new MonitorForm(game));

            /*
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
            */ 
            //monitor.GameOver();
            Console.ReadKey();
        }
    }
}