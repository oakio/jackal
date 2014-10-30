using System;
using System.IO;
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
            DrawTileBackground(tile, ship);
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

	    private void DrawTileBackground(Tile tile, Ship ship)
	    {
	        TileType type = tile.Type;

	        // не зарисовываем корабль водой
	        if (ship != null)
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

	        int rotateCount = 0;

	        string filename;
	        switch (type)
	        {
	            case TileType.Unknown:
	                isUnknown = true;
                    filename = @"back";
	                break;
	            case TileType.Water:
                    filename = @"water";
	                break;
	            case TileType.Grass:
                    filename = @"empty1";
	                break;
	            case TileType.Chest1:
                    filename = @"chest1";
	                break;
	            case TileType.Chest2:
                    filename = @"chest2";
	                break;
	            case TileType.Chest3:
                    filename = @"chest3";
	                break;
	            case TileType.Chest4:
                    filename = @"chest4";
	                break;
	            case TileType.Chest5:
                    filename = @"chest5";
	                break;
	            case TileType.Fort:
                    filename = @"fort";
	                break;
	            case TileType.RespawnFort:
                    filename = @"respawn";
	                break;
	            case TileType.RumBarrel:
                    filename = @"rumbar";
	                break;
	            case TileType.Horse:
                    filename = @"horse";
	                break;
                case TileType.Croc:
                    filename = @"croc";
                    break;
                case TileType.Airplane:
                    filename = @"airplane";
                    break;
                case TileType.Balloon:
                    filename = @"balloon";
                    break;
                case TileType.Ice:
                    filename = @"ice";
                    break;
	            case TileType.Arrow:
	                var search = ArrowsCodesHelper.Search(tile.ArrowsCode);
	                rotateCount = search.RotateCount;
	                int fileNumber = (search.ArrowType + 1);
	                filename = @"arrow" + fileNumber;
	                break;
	            default:
	                throw new NotSupportedException();
	        }

            string relativePath = string.Format(@"Content\Fields\{0}.png", filename);

	        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
	        var image = Image.FromFile(Path.Combine(baseDir, relativePath));
	        switch (rotateCount)
	        {
	            case 1:
	                image.RotateFlip(RotateFlipType.Rotate90FlipNone);
	                break;
	            case 2:
	                image.RotateFlip(RotateFlipType.Rotate180FlipNone);
	                break;
	            case 3:
	                image.RotateFlip(RotateFlipType.Rotate270FlipNone);
	                break;
	        }
	        BackgroundImage = image;
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
