using System;
using System.Linq;
using System.Collections.Generic;
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

        public void Draw(Tile tile, List<Ship> ships)
        {
            var pirates = tile.Pirates;
            int coins = tile.Coins;
            foreach (var ship in ships)
            {
                if (tile.Position == ship.Position)
                {
                    pirates = ship.Crew;
                    coins = ship.Coins;
                    break;
                }
            }
            DrawPirates(pirates);
            DrawGold(coins);

            if (isUnknown == false && tile.Type != TileType.Unknown)
            {
                return;
            }

            if (isUnknown && tile.Type != TileType.Unknown)
            {
                isUnknown = false;
            }

            string relativePath = "";
            switch(tile.Type)
            {
                case TileType.Unknown:
                    isUnknown = true;
                    relativePath = @"Content\Fields\back.png";
                    break;
                case TileType.Water:
                    relativePath = @"Content\Fields\water.png";
                    break;
                case TileType.Grass:
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

        private void DrawPirates(HashSet<Pirate> pirates)
        {
            if (pirates == null || pirates.Count == 0)
            {
                this.lblPirates.Visible = false;
                return;
            }

            lblPirates.Visible = true;
            lblPirates.BackColor = GetTeamColor(pirates.First().TeamId);
            lblPirates.Text = pirates.Count > 1 ? pirates.Count.ToString() + "P" : "P";
        }

        private void DrawGold(int goldCount)
        {
            if (goldCount == 0)
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
