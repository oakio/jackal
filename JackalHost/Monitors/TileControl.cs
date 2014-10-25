using System;
using System.Drawing;
using System.Windows.Forms;
using Jackal;

namespace JackalHost.Monitors
{
	public partial class TileControl : UserControl
	{
		public TileControl()
		{
			InitializeComponent();
		}

        public void Draw(TileType type)
        {
            string relativePath = "";
            switch(type)
            {
                case TileType.Unknown: 
                    relativePath = @"Content\Fields\back.png";
                    break;
                case TileType.Water:
                    relativePath = @"Content\Fields\water.png";
                    break;
                case TileType.Grass:
                    //Random rnd = new Random();
                    //int index = rnd.Next(1, 5);
                    //relativePath = @"Content\Fields\empty" + index + @".png";
                    relativePath = @"Content\Fields\empty1.png";
                    break;
		        case TileType.Chest1:
                    relativePath = @"Content\Fields\chest1.png";
                    break;
                case TileType.Chest2:
                    relativePath = @"Content\Fields\chest2.png";
                    break;
                case TileType.Chest3:
                    relativePath = @"Content\Fields\chest3.png";
                    break;
                case TileType.Chest4:
                    relativePath = @"Content\Fields\chest4.png";
                    break;
                case TileType.Chest5:
                    relativePath = @"Content\Fields\chest5.png";
                    break;
                default: 
                    throw new NotSupportedException();
            }

            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            this.BackgroundImage = Image.FromFile(baseDir + relativePath);
        }

		public void DrawGold(int goldCount, Tile tile)
		{
			if (goldCount > 0)
			{
                lblGold.Text = goldCount.ToString();
                lblGold.Visible = true;
			}
            else
            {
                lblGold.Visible = false;
            }
		}
	}
}
