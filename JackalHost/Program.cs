using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Jackal;
using Jackal.Players;
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
        private static int prevTurnes = 0;
        private static int nextTurnes = 0;

        private static void formStart()
        {
            Application.EnableVisualStyles();
            Application.Run(_form);
        }

		private static void Main(string[] args)
		{
			IPlayer[] players =
			{
				new SmartPlayerDistCalc(), 
				new SmartPlayerDistCalc(),
				new SmartPlayerDistCalc(),
				new SmartPlayerDistCalc(),
			};
			int mapId = 987412+1;
            board = new Board(mapId);
            game = new Game(players, board);

            _form = new MonitorForm(game, mapId);
            _form.OnCloseBtnClick += (s, e) =>
            {
                Environment.Exit(0);
            };
            _form.OnPauseBtnClick += (s, e) =>
            {
                isPause = !isPause;
            };
            _form.OnSlowerBtnClick += (s, e) =>
            {
                turnTimeOutInMS = (int) (turnTimeOutInMS*1.5);
                turnTimeOutInMS = Math.Min(turnTimeOutInMS, 2048);
            };
            _form.OnFasterBtnClick += (s, e) =>
            {
                turnTimeOutInMS = (int)(turnTimeOutInMS / 1.5);
                turnTimeOutInMS = Math.Max(turnTimeOutInMS, 2);
            };
            _form.OnNewGameBtnClick += (s, e) => {
                mapId = new Random().Next(1000000);
                board = new Board(mapId);
                game = new Game(players, board);
                _form.InitBoardPanel(game, mapId);
                isPause = false;
            };
            _form.OnPrevTurnesBtnClick += (s, e) =>
            {
                isPause = true;
                prevTurnes = 4;
            };
            _form.OnPrevOneBtnClick += (s, e) =>
            {
                isPause = true;
                prevTurnes = 1;
            };
            _form.OnNextOneBtnClick += (s, e) =>
            {
                isPause = true;
                nextTurnes = 1;
            };
            _form.OnNextTurnesBtnClick += (s, e) =>
            {
                isPause = true;
                nextTurnes = 4;
            };

            var thread = new Thread(formStart);
            thread.Start();

            while (true)
            {
                var boardStrHistory = new Dictionary<long, string>();
                for (long i = 0, prev_i = 0; !game.IsGameOver; prev_i = i, i++)
                {
                    while (isPause)
                    {
                        if (prevTurnes > 0)
                        {                        
                            i -= i >= 2 ? 2 : 0;
                            prevTurnes--;
                            break;
                        }
                        if (nextTurnes > 0)
                        {
                            nextTurnes--;
                            break;
                        }
                        Thread.Sleep(TimeSpan.FromMilliseconds(250));
                    }

                    string prevBoardStr = JsonHelper.SerialiazeWithType(board);
                    if (i >= boardStrHistory.Count)
                    {                    
                        boardStrHistory.Add(i, prevBoardStr);
                        game.Turn();

                        var prevBoard = JsonHelper.DeserialiazeWithType<Board>(prevBoardStr);
                        _form.Draw(board, prevBoard);
                    }
                    else
                    {
                        string historyStr = boardStrHistory[i];
                        var history = JsonHelper.DeserialiazeWithType<Board>(historyStr);
                        
                        var prevBoard = board;
                        if(i > prev_i)
                        {
                            string prevHistoryStr = boardStrHistory[0];
                            prevBoard = JsonHelper.DeserialiazeWithType<Board>(prevHistoryStr);
                        }
                        _form.Draw(history, prevBoard);         
                    }

                    _form.DrawStats(game);
                    Thread.Sleep(TimeSpan.FromMilliseconds(turnTimeOutInMS));
                }
                Thread.Sleep(TimeSpan.FromMilliseconds(250));
            }
		}
	}
}