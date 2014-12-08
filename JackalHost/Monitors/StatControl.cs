using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Jackal;

namespace JackalHost.Monitors
{
	public partial class StatControl : UserControl
	{
		public StatControl()
		{
			InitializeComponent();
		}

        public void DrawTurn(Game game)
        {
            txtBox.BackColor = Color.White;
            txtBox.ForeColor = Color.Black;
            txtBox.Text = "MapId: " + game.Board.MapId + Environment.NewLine;
            txtBox.Text += "TurnNo: " + game.TurnNo + (game.IsGameOver ? " - game over" : "");
            txtBox.Text += Environment.NewLine + "gold = " + game.CoinsLeft;
        }

        public void DrawStat(Team team, int goldCount)
        {
            txtBox.BackColor = TileControl.GetTeamColor(team.Id);
            txtBox.Text = string.Format(
                "{0}:{1}gold = {2}", team.Name, Environment.NewLine, goldCount
            );
        }
	}
}
