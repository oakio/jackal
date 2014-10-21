using System;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using Jackal;

namespace JackalHost.Monitors
{
	public partial class MonitorForm : Form
	{
		private readonly Game _game;
		private readonly Board _board;

		public MonitorForm(Game game)
		{
			_game = game;
			_board = game.Board;
			InitializeComponent();
		}

		private void MonitorForm_Load(object sender, EventArgs e)
		{
			splitContainer.Panel1.Controls.Clear();

			for (int y = 0; y < Board.Size; y++)
			{
				for (int x = 0; x < Board.Size; x++)
				{
					var tileControl = new TileControl
					{
						Name = GetTileKey(x, y), 
						Location = new Point(x*56, y*56)
					};
					splitContainer.Panel1.Controls.Add(tileControl);
				}
			}

			for (int i = 0; i < _game.Board.Teams.Length; i++)
			{
				var teamStatControl = new StatControl
				{
					Name = GetStatKey(i),
					Location = new Point(0, (i + 1)*36)
				};
				splitContainer.Panel2.Controls.Add(teamStatControl);
			}

		    timer1.Enabled = true;
		}

        public void Draw(bool isGameOver=false)
		{
            DrawStats(isGameOver);

			for (int y = 0; y < Board.Size; y++)
			{
				for (int x = 0; x < Board.Size; x++)
				{
					var tileControl = splitContainer.Panel1.Controls[GetTileKey(x, y)] as TileControl;
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

					DrawTile(tileControl, backColor, goldCount, piratesCount);
				}
			}

			splitContainer.Panel1.Invalidate();
		}

        private void DrawStats(bool isGameOver = false)
		{
			for (int i = 0; i < _game.Board.Teams.Length; i++)
			{
				var statControl = splitContainer.Panel2.Controls[GetStatKey(i)] as StatControl;
				if (statControl == null)
				{
					continue;
				}

				var team = _game.Board.Teams[i];
				int goldCount;
				_game.Scores.TryGetValue(team.Id, out goldCount);
				DrawStat(statControl, team.Id, goldCount);
			}

			DrawTurn(isGameOver);
		}

		private void DrawStat(StatControl statControl, int teamId, int goldCount)
		{
            /*
			if (InvokeRequired == false)
			{
				return;
			}
            */

			//statControl.Invoke((MethodInvoker)delegate
			//{
				statControl.txtBox.BackColor = GetTeamColor(teamId);
				statControl.txtBox.Text = string.Format("Team {0}: gold = {1}", teamId, goldCount);
			//});				
		}

        private void DrawTurn(bool isGameOver = false)
		{
            /*
			if (InvokeRequired == false)
			{
				return;
			}
             */ 

			//txtTurn.Invoke((MethodInvoker)delegate
			//{
            txtTurn.Text = "TurnNo: " + _game.TurnNo + (isGameOver ? " - game over" : "");
            //});			
		}

	    private void DrawTile(TileControl tileControl,
	        Color backColor,
	        int goldCount,
	        int piratesCount)
	    {
	        /*
			if (InvokeRequired == false)
			{
				return;
			}
             **/

	        //tileControl.Invoke((MethodInvoker) delegate
	        //{

	        tileControl.BackColor = backColor;

	        string goldText = "";
	        if (goldCount > 0)
	            goldText = goldCount.ToString() + " o";
	        tileControl.lblGold.Text = goldText;

	        string piratesText = "";
	        if (piratesCount == 1)
	            piratesText = "P";
	        else if (piratesCount > 1)
	            piratesText = piratesCount.ToString() + "P";
	        tileControl.lblPirates.Text = piratesText;
	        //});
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (game.TurnNo%100 == 0)
            Draw();
            
            //Thread.Sleep(TimeSpan.FromMilliseconds(1));
            //Console.ReadKey();
            _game.Turn();
            if (_game.IsGameOver)
            {
                Draw(true);
                timer1.Enabled = false;
            }
        }
	}
}
