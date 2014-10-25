using System;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using Jackal;

namespace JackalHost.Monitors
{
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

        private void gameSplitContainer_Panel1_Resize(object sender, EventArgs e)
        {
            gameSplitContainer.Panel1.Controls.Clear();
            int tileWidth = gameSplitContainer.Panel1.Width / Board.Size - 2;
            int tileHeight = gameSplitContainer.Panel1.Height / Board.Size - 2;

            for (int y = 0; y < Board.Size; y++)
            {
                for (int x = 0; x < Board.Size; x++)
                {
                    var tileControl = new TileControl
                    {
                        Name = GetTileKey(x, y),
                        Location = new Point(x * (tileWidth + 2), y * (tileHeight + 2)),
                        Size = new Size(tileWidth, tileHeight)
                    };
                    tileControl.lblPirates.Size = new Size(tileWidth, tileHeight / 2);
                    tileControl.lblGold.Size = new Size(tileWidth, tileHeight / 2);
                    gameSplitContainer.Panel1.Controls.Add(tileControl);
                }
            }

            Draw();
        }

        private void statSplitContainer_Panel1_Resize(object sender, EventArgs e)
        {
            statSplitContainer.Panel1.Controls.Clear();
            int statWidth = statSplitContainer.Panel1.Width;
            int statHeight = statSplitContainer.Panel1.Height / STAT_COUNT;

            for (int i = 0; i < STAT_COUNT; i++)
            {
                var teamStatControl = new StatControl
                {
                    Name = GetStatKey(i),
                    Width = statWidth,
                    Height = statHeight,
                    Location = new Point(0, i * statHeight),
                    Anchor = AnchorStyles.Left | AnchorStyles.Right
                };
                statSplitContainer.Panel1.Controls.Add(teamStatControl);
            }

            DrawStats(false);
        }

        public void Draw(bool isGameOver = false)
		{
            DrawStats(isGameOver);

			for (int y = 0; y < Board.Size; y++)
			{
				for (int x = 0; x < Board.Size; x++)
				{
					var tileControl = gameSplitContainer.Panel1.Controls[GetTileKey(x, y)] as TileControl;
					if (tileControl == null)
					{
						continue;
					}

					var tile = _board.Map[x, y];
					var backColor = GetTileColor(tile);
					int goldCount = tile.Coins > 0 ? tile.Coins : 0;
					int piratesCount = 0;

					foreach (var team in _board.Teams)
					{
						var ship = team.Ship;
						var position = new Position(x, y);
						if (ship.Position == position)
						{
							backColor = GetTeamColor(team.Id);
							piratesCount = ship.Crew.Count;
						    goldCount = ship.Coins;
							break;
						}
						foreach (var pirate in team.Pirates)
						{
							if (pirate.Position == position)
							{
								backColor = GetTeamColor(team.Id);
								piratesCount++;
							}
						}     
					}

					DrawTile(tileControl, tile, backColor, goldCount, piratesCount);
				}
			}

			gameSplitContainer.Panel1.Invalidate();
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
			statControl.txtBox.BackColor = GetTeamColor(teamId);
			statControl.txtBox.Text = string.Format("Team {0}: gold = {1}", teamId, goldCount);			
		}

	    private void DrawTile(TileControl tileControl,
			Tile tile,
	        Color backColor,
	        int goldCount,
	        int piratesCount)
	    {
	        tileControl.BackColor = backColor;

			tileControl.DrawGold(goldCount, tile);

	        string piratesText = "";
            if (piratesCount == 1)
            {
                piratesText = "P";
            }
            else if (piratesCount > 1)
            {
                piratesText = piratesCount.ToString() + "P";
            }
	        tileControl.lblPirates.Text = piratesText;
	    }

	    private static string GetStatKey(int index)
		{
			return string.Format("index={0}", index);
		}

		private static string GetTileKey(int x, int y)
		{
			return string.Format("x={0}y={1}", x, y);
		}

		private static Color GetTileColor(Tile tile)
		{
			switch (tile.Type)
			{
				case TileType.Unknown: return Color.Gray;
				case TileType.Stone: return Color.DarkGray;
				case TileType.Water: return Color.Cyan;
				case TileType.Grass: return Color.Green;
				case TileType.Gold: return Color.Gold;
				default: throw new NotSupportedException();
			}
		}

		private static Color GetTeamColor(int teamId)
		{
			switch (teamId)
			{
				case 0: return Color.DarkRed;
				case 1: return Color.DarkBlue;
				case 2: return Color.DarkViolet;
				case 3: return Color.DarkOrange;
				default: throw new NotSupportedException();
			}
		}

        private void gameTurnTimer_Tick(object sender, EventArgs e)
        {
            Draw();
            _game.Turn();

            if (_game.IsGameOver)
            {
                Draw(true);
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

			_game.Turn();
			Draw();
		}

        private void newGameBtn_Click(object sender, EventArgs e)
        {
            _board = new Board(_mapId);
            _game = new Game(_players, _board);

            gameTurnTimer.Enabled = true;
            pauseGameBtn.Text = "Pause game";
        }
	}
}
