using System;
using System.Drawing;
using System.Windows.Forms;
using Jackal;

namespace JackalHost.Monitors
{
	public partial class TileControl : UserControl
	{
        bool isUnknown = true;

		public TileControl()
		{
			InitializeComponent();
		}

	    public double RandomValue { get; set; }

	    public void Draw(TileType type)
        {
            if (isUnknown == false && type != TileType.Unknown)
            {
                return;
            }

            if(isUnknown && type != TileType.Unknown)
            {
                isUnknown = false;
            }

            string relativePath = "";
            switch(type)
            {
                case TileType.Unknown:
                    isUnknown = true;
                    relativePath = @"Content\Fields\back.png";
                    break;
                case TileType.Water:
                    relativePath = @"Content\Fields\water.png";
                    break;
                case TileType.Grass:
                    int index = ((int) (RandomValue*4)) + 1; //rnd.Next(1, 5);
                    relativePath = @"Content\Fields\empty" + index + @".png";
                    //relativePath = @"Content\Fields\empty1.png";
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

        public void DrawPirates(int piratesCount, int teamId)
        {
            if(teamId == -1)
            {
                this.lblPirates.Visible = false;
                return;
            }

            lblPirates.Visible = true;
            lblPirates.BackColor = GetTeamColor(teamId);
            lblPirates.Text = piratesCount > 1 ? piratesCount.ToString() + "P" : "P";
        }

		public void DrawGold(int goldCount)
		{
            if(goldCount == 0)
            {
                lblGold.Visible = false;
                return;
            }

            lblGold.Text = goldCount.ToString();
            lblGold.Visible = true;
		}

        public static Color GetTeamColor(int teamId)
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
	}
}
