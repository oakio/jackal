using System.Drawing;
using System.Windows.Forms;

namespace JackalHost.Monitors
{
	public partial class TileControl : UserControl
	{
		private const int BORDER_SIZE = 1;

		public TileControl()
		{
			InitializeComponent();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			ControlPaint.DrawBorder(
				e.Graphics, ClientRectangle,
				Color.Black, BORDER_SIZE, ButtonBorderStyle.Solid,
				Color.Black, BORDER_SIZE, ButtonBorderStyle.Solid,
				Color.Black, BORDER_SIZE, ButtonBorderStyle.Solid,
				Color.Black, BORDER_SIZE, ButtonBorderStyle.Solid
			);
		}
	}
}
