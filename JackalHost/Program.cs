using System;
using System.Threading;
using System.Windows.Forms;
using Jackal;
using Jackal.Players;
using JackalHost.Monitors;

namespace JackalHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IPlayer[] players = { new MikePlayer(), new SmartPlayer(), new SmartPlayer(), new SmartPlayer() };

            const int mapId = 201;

            Application.EnableVisualStyles();
            Application.Run(new MonitorForm(players, mapId));

            /*
            var board = new Board(mapId);
            var game = new Game(players, board);
            
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
            */
        }
    }
}