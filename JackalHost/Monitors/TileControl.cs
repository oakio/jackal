using System.Drawing;
using System.Windows.Forms;

namespace JackalHost.Monitors
{
	public partial class TileControl : UserControl
	{
		//private const int BORDER_SIZE = 1;
		int maxGold = 0;

		public TileControl()
		{
			InitializeComponent();
		}

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    base.OnPaint(e);
        //    ControlPaint.DrawBorder(
        //        e.Graphics, ClientRectangle,
        //        Color.Black, BORDER_SIZE, ButtonBorderStyle.Solid,
        //        Color.Black, BORDER_SIZE, ButtonBorderStyle.Solid,
        //        Color.Black, BORDER_SIZE, ButtonBorderStyle.Solid,
        //        Color.Black, BORDER_SIZE, ButtonBorderStyle.Solid
        //    );
        //}

		public void DrawGold(int goldCount, Jackal.Tile tile)
		{
			if (goldCount > maxGold)
			{
				maxGold = goldCount;
			}

			string goldText = tile.Type == Jackal.TileType.Gold ? string.Format("{0} ({1}) o", goldCount, maxGold) : "";
			if (goldCount > 0)
			{
				goldText = string.Format("{0} ({1}) o", goldCount, maxGold);
			}
			lblGold.Text = goldText;
		}
	}
}
