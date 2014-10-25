using System.Drawing;
using System.Windows.Forms;

namespace JackalHost.Monitors
{
	public partial class TileControl : UserControl
	{
		int maxGold = 0;

		public TileControl()
		{
			InitializeComponent();
		}

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
