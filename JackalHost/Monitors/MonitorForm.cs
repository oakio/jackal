using System;
using System.Collections.Generic;
using System.Linq;
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

        public event EventHandler OnNextOneBtnClick;
        public event EventHandler OnNextTurnesBtnClick;

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

            gameSplitContainer_Panel1_Resize(this, EventArgs.Empty);
            statSplitContainer_Panel1_Resize(this, EventArgs.Empty);
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
                    Draw(tile, board.Teams.Select(item => item.Ship).ToList());
                }
            }
        }

        private void gameSplitContainer_Panel1_Resize(object sender, EventArgs e)
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

        private void statSplitContainer_Panel1_Resize(object sender, EventArgs e)
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
            var ships = board.Teams.Select(item => item.Ship).ToList();

            for (int y = 0; y < Board.Size; y++)
            {
                for (int x = 0; x < Board.Size; x++)
                {
                    var tile = board.Map[x, y];
                    var pirates = tile.Pirates;
                    var prevTile = prevBoard.Map[x, y];
                    var prevPirates = prevTile.Pirates;

                    if (pirates.Count != prevPirates.Count ||
                        tile.Coins != prevTile.Coins ||
                        tile.OccupationTeamId != prevTile.OccupationTeamId)
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
                var gamePanel = gameSplitContainer.Panel1;
                var tileKey = GetTileKey(tile.Position.X, tile.Position.Y);
                var tileControl = gamePanel.Controls[tileKey] as TileControl;
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
                        DrawTurn(statControl, game.TurnNo, game.IsGameOver);
                        continue;
                    }

                    if (i > game.Board.Teams.Length)
                    {
                        break;
                    }
                    var team = game.Board.Teams[i - 1];
                    int goldCount;
                    game.Scores.TryGetValue(team.Id, out goldCount);
                    DrawStat(statControl, team.Id, goldCount);
                }
            }
            catch (Exception)
            {
            }
        }

        private void DrawTurn(StatControl statControl, int turnNo, bool isGameOver = false)
        {
            if (statControl.InvokeRequired)
            {
                statControl.Invoke(new Action(() =>
                {
                    statControl.DrawTurn(turnNo, isGameOver);
                }));
            }
            else
            {
                statControl.DrawTurn(turnNo, isGameOver);
            }
        }

        private void DrawStat(StatControl statControl, int teamId, int goldCount)
        {
            if (statControl.InvokeRequired)
            {
                statControl.Invoke(new Action(() =>
                {
                    statControl.DrawStat(teamId, goldCount);
                }));
            }
            else
            {
                statControl.DrawStat(teamId, goldCount);
            }
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
