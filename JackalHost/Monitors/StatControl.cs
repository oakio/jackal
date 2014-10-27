using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JackalHost.Monitors
{
	public partial class StatControl : UserControl
	{
		public StatControl()
		{
			InitializeComponent();
		}

        public void DrawTurn(int turnNo, bool isGameOver = false)
        {
            txtBox.BackColor = Color.White;
            txtBox.ForeColor = Color.Black;
            txtBox.Text = "TurnNo: " + turnNo + (isGameOver ? " - game over" : "");
        }

        public void DrawStat(int teamId, int goldCount)
        {
            txtBox.BackColor = TileControl.GetTeamColor(teamId);
            txtBox.Text = string.Format("Team {0}: gold = {1}", teamId, goldCount);
        }
	}
}
