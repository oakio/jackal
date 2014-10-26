using System;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using Jackal;

namespace JackalHost.Monitors
{
    /*
     *  Скажем спасибо ресурсу http://www.gamedev.ru/projects/forum/?id=190046 
     *  и лично Esper за предоставленные картинки высокого качества, 
     *  считаю что данного товарища можно пригласить в проект
    */

    public partial class MonitorForm : Form
	{
        private const int STAT_COUNT = 5;

        private readonly IPlayer[] _players;
        private readonly int _mapId;

		private Game _game;
		private Board _board;

        public MonitorForm(IPlayer[] players, int mapId)
		{
            _players = players;
            _mapId = mapId;

            _board = new Board(_mapId);
            _game = new Game(_players, _board);

			InitializeComponent();
		}

        private void MonitorForm_Load(object sender, EventArgs e)
        {
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
                    boardPanel.Controls.Add(tileControl);

                    var position = new Position(x, y);
                    DrawTile(position);
                }
            }

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
            DrawStats();

            gameSplitContainer_Panel1_Resize(this, EventArgs.Empty);
            statSplitContainer_Panel1_Resize(this, EventArgs.Empty);
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
                    var tileControl = boardPanel.Controls[GetTileKey(x, y)] as TileControl;
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

        public void Draw(Position from, Position to)
		{
            DrawTile(from);
            DrawTile(to);
            DrawStats();
		}

        private void DrawStats(bool isGameOver = false)
		{
            for (int i = 0; i < STAT_COUNT; i++)
			{
				var statControl = statSplitContainer.Panel1.Controls[GetStatKey(i)] as StatControl;
				if (statControl == null)
				{
					continue;
				}

                if(i == 0)
                {
                    DrawTurn(statControl, isGameOver);
                    continue;
                }

                if(i > _game.Board.Teams.Length)
                {
                    break;
                }
				var team = _game.Board.Teams[i - 1];
				int goldCount;
				_game.Scores.TryGetValue(team.Id, out goldCount);
				DrawStat(statControl, team.Id, goldCount);
			}
		}

        private void DrawTurn(StatControl statControl, bool isGameOver = false)
        {
            statControl.txtBox.BackColor = Color.White;
            statControl.txtBox.ForeColor = Color.Black;
            statControl.txtBox.Text = "TurnNo: " + _game.TurnNo + (isGameOver ? " - game over" : "");
        }

		private void DrawStat(StatControl statControl, int teamId, int goldCount)
		{
			statControl.txtBox.BackColor = TileControl.GetTeamColor(teamId);
			statControl.txtBox.Text = string.Format("Team {0}: gold = {1}", teamId, goldCount);			
		}

	    private void DrawTile(Position position)
	    {
            var gamePanel = gameSplitContainer.Panel1;
            var tileKey = GetTileKey(position.X, position.Y);
            var tileControl = gamePanel.Controls[tileKey] as TileControl;
            var tile = _board.Map[position.X, position.Y];

            int teamId = -1;
            int piratesCount = 0;
            int goldCount = tile.Coins > 0 ? tile.Coins : 0;

            foreach (var team in _board.Teams)
            {
                var ship = team.Ship;
                if (ship.Position == position)
                {
                    teamId = team.Id;
                    piratesCount = ship.Crew.Count;
                    goldCount = ship.Coins;
                    break;
                }
                foreach (var pirate in team.Pirates)
                {
                    if (pirate.Position == position)
                    {
                        teamId = team.Id;
                        piratesCount++;
                    }
                }
            }

            tileControl.Draw(tile.Type);
			tileControl.DrawGold(goldCount);
            tileControl.DrawPirates(piratesCount, teamId);
	    }

	    private static string GetStatKey(int index)
		{
			return string.Format("index={0}", index);
		}

		private static string GetTileKey(int x, int y)
		{
			return string.Format("x={0}y={1}", x, y);
		}

        private void gameTurnTimer_Tick(object sender, EventArgs e)
        {
            var move = _game.Turn();
            Draw(move.From, move.To);

            if (_game.IsGameOver)
            {
                DrawStats(true);
                gameTurnTimer.Enabled = false;
            }
        }

        private void pauseGameBtn_Click(object sender, EventArgs e)
        {
            if (gameTurnTimer.Enabled)
            {
                gameTurnTimer.Enabled = false;
                pauseGameBtn.Text = "Start game";
            }
            else
            {
                gameTurnTimer.Enabled = true;
                pauseGameBtn.Text = "Pause game";
            }
        }

		private void oneTurnBtn_Click(object sender, EventArgs e)
		{
			if (gameTurnTimer.Enabled)
			{
				gameTurnTimer.Enabled = false;
				pauseGameBtn.Text = "Start game";
			}

			var move = _game.Turn();
            Draw(move.From, move.To);
		}

        private void newGameBtn_Click(object sender, EventArgs e)
        {
            _board = new Board(_mapId);
            _game = new Game(_players, _board);

            MonitorForm_Load(this, EventArgs.Empty);
            gameTurnTimer.Enabled = true;
            pauseGameBtn.Text = "Pause game";
        }
	}
}
