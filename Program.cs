using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Jackal;
using JackalHost.Monitors;

namespace JackalHost
{
    internal class Program
    {
        private static MonitorForm _form;

        private static void formStart()
        {
            Application.EnableVisualStyles();
            Application.Run(_form);
        }

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

            _form = new MonitorForm(game);
            var thread = new Thread(formStart);
            thread.Start();

            while (!game.IsGameOver)
            {
                var move = game.Turn();

                var ships = board.Teams.Select(item => item.Ship).ToList();
                var fromTile = board.Map[move.From.X, move.From.Y];
                var toTile = board.Map[move.To.X, move.To.Y];

                _form.Draw(fromTile, ships);
                _form.Draw(toTile, ships);
                _form.DrawStats(game);

                Thread.Sleep(TimeSpan.FromMilliseconds(1));
            }
            Console.ReadKey();
        }
    }
}