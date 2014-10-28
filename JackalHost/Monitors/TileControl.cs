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
        private bool isUnknown = true;

        public double RandomValue { get; set; }

		public TileControl()
		{
			InitializeComponent();
		}

        public void Draw(Tile tile, List<Ship> ships)
        {
            var ship = ships.FirstOrDefault(item => item.Position == tile.Position);
            if(ship != null)
            {
                BackgroundImage = null;
                BackColor = GetTeamColor(ship.TeamId);
            }
            else if (tile.Type == TileType.Water && BackgroundImage == null)
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string relativePath = @"Content\Fields\water.png";
                BackgroundImage = Image.FromFile(baseDir + relativePath);
                BackColor = SystemColors.Control;
            }

            DrawPirates(ship == null ? tile.Pirates : ship.Crew);
            DrawGold(ship == null ? tile.Coins : ship.Coins);
            DrawTileBackground(tile.Type, ship);
        }

	    private void DrawPirates(HashSet<Pirate> pirates)
	    {
	        if (pirates == null || pirates.Count == 0)
	        {
	            lblPirates.Visible = false;
	            return;
	        }

	        lblPirates.Visible = true;
	        lblPirates.BackColor = GetTeamColor(pirates.First().TeamId);
	        string str = "";
	        const char drunkChar = '☺';
	        int drunkCount = pirates.Count(x => x.IsDrunk);
	        if (drunkCount > 0)
	            str += (drunkCount > 1 ? drunkCount.ToString() : "") + drunkChar;
	        int normalCount = pirates.Count(x => x.IsDrunk == false);
	        if (normalCount>0)
	            str += (normalCount > 1 ? normalCount.ToString() : "") + "P";
	        lblPirates.Text = str;
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

        private void DrawTileBackground(TileType type, Ship ship)
        {
            // не зарисовываем корабль водой
            if(ship != null)
            {
                return;
            }

            // после открытия поля - фон не меняется
            if (isUnknown == false && type != TileType.Unknown)
            {
                return;
            }
            if (isUnknown && type != TileType.Unknown)
            {
                isUnknown = false;
            }

            string relativePath = "";
            switch (type)
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
                case TileType.Fort:
                    relativePath = @"Content\Fields\fort.png";
                    break;
                case TileType.RespawnFort:
                    relativePath = @"Content\Fields\respawn.png";
                    break;
                case TileType.RumBarrel:
                    relativePath = @"Content\Fields\rumbar.png";
                    break;
                default:
                    throw new NotSupportedException();
            }

            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            BackgroundImage = Image.FromFile(baseDir + relativePath);
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
