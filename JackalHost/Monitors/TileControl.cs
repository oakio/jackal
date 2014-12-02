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

		public TileControl()
		{
			InitializeComponent();
		}

        public void Draw(Tile tile, List<Ship> ships)
        {
            var tileShip = ships.FirstOrDefault(item => item.Position == tile.Position);
            if (tileShip != null)
            {
                BackgroundImage = null;
                BackColor = GetTeamColor(tileShip.TeamId);
            }
            else if (tile.Type == TileType.Water && BackgroundImage == null)
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string relativePath = @"Content\Fields\water.png";
                BackgroundImage = Image.FromFile(baseDir + relativePath);
                BackColor = SystemColors.Control;
            }

            Controls.Clear();
            for (int i = 0; i < tile.Levels.Count; i++)
            {
                var level = tile.Levels[i];
                DrawPiratesAndCoins(level, i, tile.Levels.Count, tileShip);
            }
            DrawTileBackground(tile, tileShip);
        }

        private void DrawPiratesAndCoins(TileLevel level, int levelIndex, int levelCount, Ship tileShip)
	    {
            var pirates = level.Pirates;
            bool hasPirates = pirates != null && pirates.Count > 0;
            bool hasCoins = (tileShip != null && tileShip.Coins > 0) || level.Coins > 0;

            int locX = 0;
            int locY = 0;
            int width = Width / 4;
            int height = Height / 4;

            // calc location
            switch(levelCount)
            {
                case 5:
                {
                    if (levelIndex == 4)
                    {
                        locX = hasCoins ? 2 * width : 3 * width;
                        locY = 0;
                    }
                    if (levelIndex == 3)
                    {
                        locX = hasCoins ? 0 : width;
                        locY = 0;
                    }
                    if (levelIndex == 2)
                    {
                        locX = 0;
                        locY = (int)(1.4 * height);
                    }
                    if (levelIndex == 1)
                    {
                        locX = 0;
                        locY = 3 * height;
                    }
                    if (levelIndex == 0)
                    {
                        locX = hasCoins ? 2 * width : 3 * width;
                        locY = 3 * height;
                    }
                    break;
                }
                case 4:
                {
                    if (levelIndex == 3)
                    {
                        locX = hasCoins ? 2 * width : 3 * width;
                        locY = 3 * height;
                    }
                    if (levelIndex == 2)
                    {
                        locX = 0;
                        locY = (int)(2.4 * height);
                    }
                    if (levelIndex == 1)
                    {
                        locX = 2 * width;
                        locY = (int)(0.8 * height);
                    }
                    if (levelIndex == 0)
                    {
                        locX = 0;
                        locY = 0;
                    }
                    break;
                }
                case 3:
                {
                    if (levelIndex == 2)
                    {
                        locX = hasCoins ? 2 * width : 3 * width;
                        locY = 3 * height;
                    }
                    if (levelIndex == 1)
                    {
                        locX = width;
                        locY = (int)(1.4 * height);
                    }
                    if (levelIndex == 0)
                    {
                        locX = hasCoins ? 2 * width : 3 * width;
                        locY = 0;
                    }
                    break;
                }
                case 2:
                {
                    if (levelIndex == 1)
                    {
                        locX = 0;
                        locY = 3 * height;
                    }
                    if (levelIndex == 0)
                    {
                        locX = hasCoins ? 2 * width : 3 * width;
                        locY = 0;
                    }
                    break;
                }
                case 1:
                {
                    width = Width / 3;
                    height = Height / 3;
                    break;
                }
            }

            // draw pirates
            if (hasPirates)
            {
                var lblPirates = new Label();
                lblPirates.ForeColor = Color.White;
                lblPirates.BackColor = GetTeamColor(pirates.First().TeamId);
                lblPirates.TextAlign = ContentAlignment.MiddleCenter;
                lblPirates.Size = new Size(width, height);

                if (levelCount == 1)
                {
                    lblPirates.Font = new Font("Microsoft Sans Serif", 12);
                }
                lblPirates.Location = new Point(locX, locY);

                string str = "";
                const char loveChar = '❤';
                int inLoveCount = pirates.Count(x => x.IsInLove);
                if (inLoveCount > 0)
                    str += (inLoveCount > 1 ? inLoveCount.ToString() : "") + loveChar;

                const char drunkChar = '☺';
                int drunkCount = pirates.Count(x => x.IsDrunk);
                if (drunkCount > 0)
                    str += (drunkCount > 1 ? drunkCount.ToString() : "") + drunkChar;

                const char inTrapChar = '☹';
                int inTrapCount = pirates.Count(x => x.IsInTrap);
                if (inTrapCount > 0)
                    str += (inTrapCount > 1 ? inTrapCount.ToString() : "") + inTrapChar;

                int normalCount = pirates.Count(x => x.IsDrunk == false && x.IsInTrap == false);
                if (normalCount > 0)
                    str += (normalCount > 1 ? normalCount.ToString() : "") + "P";

                lblPirates.Text = str;
                Controls.Add(lblPirates);
            }

            // draw coins
            if (hasCoins)
            {
                int coins = tileShip != null ? tileShip.Coins : level.Coins;

                var lblGold = new Label();
                lblGold.ForeColor = Color.Black;
                lblGold.BackColor = Color.Gold;
                lblGold.TextAlign = ContentAlignment.MiddleCenter;
                lblGold.Size = new Size(width, height);

                if (levelCount == 1)
                {
                    lblGold.Font =  new Font("Microsoft Sans Serif", coins > 9 ? 9 : 12);
                    locX = 2 * width;
                }
                else if (hasPirates)
                {
                    locX = locX + width;
                }
                lblGold.Location = new Point(locX, locY);
                lblGold.Text = coins.ToString();
                Controls.Add(lblGold);
            }
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
                case TileType.Trap:
                    filename = @"trap";
                    break;
                case TileType.Canibal:
                    filename = @"canibal";
                    break;
                case TileType.Spinning:
	                switch (tile.SpinningCount)
	                {
	                    case 2:
	                        filename = "forest";
	                        break;
	                    case 3:
	                        filename = "desert";
	                        break;
	                    case 4:
	                        filename = "swamp";
	                        break;
	                    case 5:
	                        filename = "mount";
	                        break;
	                    default:
	                        throw new NotSupportedException();
	                }
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
