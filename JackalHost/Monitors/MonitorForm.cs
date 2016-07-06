﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using Jackal;

namespace JackalHost.Monitors
{
	/*
	 *  Скажем спасибо ресурсу http://www.gamedev.ru/projects/forum/?id=190046 
	 *  и лично Esper за предоставленные картинки высокого качества
	*/

	public partial class MonitorForm : Form
	{
		private const int STAT_COUNT = 5;
        private readonly Game _game;
        private int _mapId;

        public event EventHandler OnCloseBtnClick;
        public event EventHandler OnPauseBtnClick;
        public event EventHandler OnSlowerBtnClick;
        public event EventHandler OnFasterBtnClick;
        public event EventHandler OnNewGameBtnClick;

        public event EventHandler OnPrevOneBtnClick;
        public event EventHandler OnPrevTurnesBtnClick;
        public event EventHandler OnNextOneBtnClick;
        public event EventHandler OnNextTurnesBtnClick;

        public event MouseEventHandler OnTileCtrlBtnClick;

        public MonitorForm(Game game, int mapId)
		{
            _game = game;
            _mapId = mapId;
			InitializeComponent();
		}

		private void MonitorForm_Load(object sender, EventArgs e)
		{
            ToolTip toolTip = new ToolTip();
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;

            var boardPanel = gameSplitContainer.Panel1;
            boardPanel.Controls.Clear();
            for (int y = 0; y < Board.Size; y++)
            {
                for (int x = 0; x < Board.Size; x++)
                {
                    var tileControl = new TileControl
                    {
                        Name = GetTileKey(x, y)
                    };
                    tileControl.MouseClick += OnTileCtrlBtnClick;
                    string positionText = string.Format("{0},{1}", x, y);
                    toolTip.SetToolTip(tileControl, positionText);
                    boardPanel.Controls.Add(tileControl);
                }
            }
            InitBoardPanel(_game, _mapId);

            var statPanel = statSplitContainer.Panel1;
            statPanel.Controls.Clear();
            for (int i = 0; i < STAT_COUNT; i++)
            {
                var statControl = new StatControl
                {
                    Name = GetStatKey(i),
                };
                statPanel.Controls.Add(statControl);
            }
            DrawStats(_game);
            gameSplitContainer_SplitterMoved(this, null);
        }

        public void InitBoardPanel(Game game, int mapId)
        {
            Random random = new Random(mapId);
            var boardPanel = gameSplitContainer.Panel1;

            for (int y = 0; y < Board.Size; y++)
            {
                for (int x = 0; x < Board.Size; x++)
                {
                    var tileControl = boardPanel.Controls[GetTileKey(x, Board.Size - 1 - y)] as TileControl;
                    if (tileControl == null)
                    {
                        continue;
                    }

                    var board = game.Board;
                    var tile = board.Map[x, y];
                    var ships = board.Teams.Select(item => item.Ship).ToList();
                    Draw(tile, ships);
                }
            }
        }

        private void MonitorForm_ResizeBegin(object sender, EventArgs e)
        {
            gameSplitContainer.SplitterMoved -= gameSplitContainer_SplitterMoved;
            statSplitContainer.SplitterMoved -= statSplitContainer_SplitterMoved;
        }

        private void MonitorForm_ResizeEnd(object sender, EventArgs e)
        {
            gameSplitContainer_SplitterMoved(this, null);
            gameSplitContainer.SplitterMoved += gameSplitContainer_SplitterMoved;
            statSplitContainer.SplitterMoved += statSplitContainer_SplitterMoved;
        }

        private void gameSplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            gameSplitContainerResize();
            statSplitContainerResize();
        }

        private void statSplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            statSplitContainerResize();
        }

        private void gameSplitContainerResize()
        {
            var boardPanel = gameSplitContainer.Panel1;
            int tileWidth = boardPanel.Width / Board.Size - 2;
            int tileHeight = boardPanel.Height / Board.Size - 2;

            for (int y = 0; y < Board.Size; y++)
            {
                for (int x = 0; x < Board.Size; x++)
                {
                    var tileControl = boardPanel.Controls[GetTileKey(x, Board.Size - 1 - y)] as TileControl;
                    if (tileControl == null)
                    {
                        continue;
                    }
                    tileControl.Size = new Size(tileWidth, tileHeight);
                    tileControl.Location = new Point(x * (tileWidth + 2), y * (tileHeight + 2));
                }
            }
        }

        private void statSplitContainerResize()
        {
            var statPanel = statSplitContainer.Panel1;
            int statWidth = statPanel.Width;
            int statHeight = statPanel.Height / STAT_COUNT;

            for (int i = 0; i < STAT_COUNT; i++)
            {
                var statControl = statPanel.Controls[GetStatKey(i)] as StatControl;
                if (statControl == null)
                {
                    continue;
                }
                statControl.Size = new Size(statWidth, statHeight);
                statControl.Location = new Point(0, i * statHeight);
            }
        }

        public void Draw(Board board, Board prevBoard)
        {
            // координаты клеток, где поменялось расположение пиратов
            var diffPositions = new HashSet<Position>();
            var idList = board.AllPirates.Union(prevBoard.AllPirates).Select(x => x.Id).Distinct();
            foreach (var guid in idList)
            {
                var newPirate = board.AllPirates.FirstOrDefault(x => x.Id == guid);
                var oldPirate = prevBoard.AllPirates.FirstOrDefault(x => x.Id == guid);
                if (newPirate == null)
                {
                    diffPositions.Add(oldPirate.Position.Position);
                }
                else if (oldPirate == null)
                {
                    diffPositions.Add(newPirate.Position.Position);
                }
                else if (oldPirate.Position != newPirate.Position
                         || oldPirate.IsDrunk != newPirate.IsDrunk
                         || oldPirate.IsInTrap != newPirate.IsInTrap
                         || oldPirate.IsInLove != newPirate.IsInLove)
                {
                    diffPositions.Add(oldPirate.Position.Position);
                    diffPositions.Add(newPirate.Position.Position);
                }
            }

            var ships = board.Teams.Select(item => item.Ship).ToList();
            for (int y = 0; y < Board.Size; y++)
            {
                for (int x = 0; x < Board.Size; x++)
                {
                    var tile = board.Map[x, y];
                    var prevTile = prevBoard.Map[x, y];
                    bool isDiffPosition = diffPositions.Any(
                        item => item == tile.Position || item == prevTile.Position
                    );

                    if (isDiffPosition
                        || tile.Type != prevTile.Type
                        || tile.Coins != prevTile.Coins)
                    {
                        Draw(tile, ships);
                    }
                }
            }
        }

        private void Draw(Tile tile, List<Ship> ships)
        {
            try
            {
                var tileControl = GetTileControl(tile.Position.X, tile.Position.Y);
                if (tileControl == null)
                {
                    return;
                }

                if (tileControl.InvokeRequired)
                {
                    tileControl.Invoke(new Action(() =>
                    {
                        tileControl.Draw(tile, ships);
                    }));
                }
                else
                {
                    tileControl.Draw(tile, ships);
                }
            }
            catch (Exception)
            {
            }
        }

        public void DrawStats(Game game)
        {
            try
            {
                for (int i = 0; i < STAT_COUNT; i++)
                {
                    var statPanel = statSplitContainer.Panel1;
                    var statControl = statPanel.Controls[GetStatKey(i)] as StatControl;
                    if (statControl == null)
                    {
                        continue;
                    }

                    if (i == 0)
                    {
                        DrawTurn(statControl, game);
                        continue;
                    }

                    if (i > game.Board.Teams.Length)
                    {
                        break;
                    }
                    var team = game.Board.Teams[i - 1];
                    int goldCount;
                    game.Scores.TryGetValue(team.Id, out goldCount);
                    DrawStat(statControl, team, goldCount);
                }
            }
            catch (Exception)
            {
            }
        }

        private void DrawTurn(StatControl statControl, Game game)
        {
            if (statControl.InvokeRequired)
            {
                statControl.Invoke(new Action(() =>
                {
                    statControl.DrawTurn(game);
                }));
            }
            else
            {
                statControl.DrawTurn(game);
            }
        }

        private void DrawStat(StatControl statControl, Team team, int goldCount)
        {
            if (statControl.InvokeRequired)
            {
                statControl.Invoke(new Action(() =>
                {
                    statControl.DrawStat(team, goldCount);
                }));
            }
            else
            {
                statControl.DrawStat(team, goldCount);
            }
        }

        public void ShowAvailableMoves(Game game)
        {
            int currentTeamId = game.CurrentTeamId;
            var availableMoves = game.GetAvailableMoves();
            foreach (var move in availableMoves)
            {
                try
                {
                    var tileControl = GetTileControl(move.To.X, move.To.Y);
                    if (tileControl == null)
                    {
                        return;
                    }

                    if (tileControl.InvokeRequired)
                    {
                        tileControl.Invoke(new Action(() =>
                        {
                            tileControl.ShowAvailableMove(currentTeamId, move.To.Level, move.WithCoins);
                        }));
                    }
                    else
                    {
                        tileControl.ShowAvailableMove(currentTeamId, move.To.Level, move.WithCoins);
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public void HideAvailableMoves(Game game)
        {
            var prevAvailableMoves = game.GetPrevAvailableMoves();
            foreach (var move in prevAvailableMoves)
            {
                try
                {
                    var tileControl = GetTileControl(move.To.X, move.To.Y);
                    if (tileControl == null)
                    {
                        return;
                    }

                    if (tileControl.InvokeRequired)
                    {
                        tileControl.Invoke(new Action(() =>
                        {
                            tileControl.HideAvailableMove();
                        }));
                    }
                    else
                    {
                        tileControl.HideAvailableMove();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private TileControl GetTileControl(int x, int y)
        {
            var gamePanel = gameSplitContainer.Panel1;
            var tileKey = GetTileKey(x, y);
            return gamePanel.Controls[tileKey] as TileControl;
        }

        private static string GetStatKey(int index)
        {
            return string.Format("index={0}", index);
        }

        private static string GetTileKey(int x, int y)
        {
            return string.Format("x={0}y={1}", x, y);
        }

		private void pauseGameBtn_Click(object sender, EventArgs e)
		{
            OnPauseBtnClick(this, EventArgs.Empty);
            pauseGameBtn.Text = pauseGameBtn.Text == "Start game" ? "Pause game" : "Start game";
		}

        private void slowTurnesBtn_Click(object sender, EventArgs e)
        {
            OnSlowerBtnClick(this, EventArgs.Empty);
        }

        private void fastTurnesBtn_Click(object sender, EventArgs e)
        {
            OnFasterBtnClick(this, EventArgs.Empty);
        }

		private void newGameBtn_Click(object sender, EventArgs e)
		{
            OnNewGameBtnClick(this, EventArgs.Empty);
            pauseGameBtn.Text = "Pause game";
		}

        private void prevTurnesBtn_Click(object sender, EventArgs e)
        {
            OnPrevTurnesBtnClick(this, EventArgs.Empty);
            pauseGameBtn.Text = "Start game";
        }

        private void prevOneBtn_Click(object sender, EventArgs e)
        {
            OnPrevOneBtnClick(this, EventArgs.Empty);
            pauseGameBtn.Text = "Start game";
        }

        private void nextOneBtn_Click(object sender, EventArgs e)
        {
            OnNextOneBtnClick(this, EventArgs.Empty);
            pauseGameBtn.Text = "Start game";
        }

        private void nextTurnesBtn_Click(object sender, EventArgs e)
        {
            OnNextTurnesBtnClick(this, EventArgs.Empty);
            pauseGameBtn.Text = "Start game";
        }

        private void MonitorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnCloseBtnClick(this, EventArgs.Empty);
        }
	}
}
