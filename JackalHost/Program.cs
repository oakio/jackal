using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Jackal;
using JackalHost.Monitors;

namespace JackalHost
{
	internal class Program
	{
        private static MonitorForm _form;
        private static Board board;
        private static Game game;

        private static bool isPause = true;
        private static int turnTimeOutInMS = 128;
        private static int nextTurnesCount = 0;

        private static void formStart()
        {
            Application.EnableVisualStyles();
            Application.Run(_form);
        }

		private static void Main(string[] args)
		{
			IPlayer[] players =
			{
				new SmartPlayer(), 
				new SmartPlayer(),
				new SmartPlayer(),
				new SmartPlayer(),
			};
			int mapId = 987412+1;
            board = new Board(mapId);
            game = new Game(players, board);

            _form = new MonitorForm(game, mapId);
            _form.OnPauseBtnClick += (s, e) =>
            {
                isPause = !isPause;
            };
            _form.OnSlowerBtnClick += (s, e) =>
            {
                turnTimeOutInMS = turnTimeOutInMS < 2048 ? turnTimeOutInMS * 2 : 2048;
            };
            _form.OnFasterBtnClick += (s, e) =>
            {
                turnTimeOutInMS = turnTimeOutInMS > 2 ? turnTimeOutInMS / 2 : 2;
            };
            _form.OnNewGameBtnClick += (s, e) => {
                mapId = new Random().Next(1000000);
                board = new Board(mapId);
                game = new Game(players, board);
                _form.InitBoardPanel(game, mapId);
                isPause = false;
            };
            _form.OnNextOneBtnClick += (s, e) =>
            {
                isPause = true;
                nextTurnesCount = 1;
            };
            _form.OnNextTurnesBtnClick += (s, e) =>
            {
                isPause = true;
                nextTurnesCount = 4;
            };

            var thread = new Thread(formStart);
            thread.Start();

            while (true)
            {
                while (!game.IsGameOver)
                {
                    while (isPause)
                    {
                        if (nextTurnesCount > 0)
                        {
                            nextTurnesCount--;
                            break;
                        }

                        Thread.Sleep(TimeSpan.FromMilliseconds(250));
                    }

                    var move = game.Turn();

                    var ships = board.Teams.Select(item => item.Ship).ToList();
                    var fromTile = board.Map[move.From.X, move.From.Y];
                    var toTile = board.Map[move.To.X, move.To.Y];

                    _form.Draw(fromTile, ships);
                    _form.Draw(toTile, ships);
                    _form.DrawStats(game);

                    Thread.Sleep(TimeSpan.FromMilliseconds(turnTimeOutInMS));
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(250));
            }
		}
	}
}