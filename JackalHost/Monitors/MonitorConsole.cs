using System;
using System.Linq;
using Jackal;

namespace JackalHost.Monitors
{
    // todo obsolete
    public class MonitorConsole
    {
        private readonly Game _game;
        private readonly Board _board;
        private readonly string _horizontalLine = new string('=', Console.WindowWidth);

        public MonitorConsole(Game game)
        {
            _game = game;
            _board = game.Board;
        }

        public void Draw()
        {
            Console.ResetColor();
            Console.SetCursorPosition(0, 0);
            DrawStatsTable();
            Console.WriteLine(_horizontalLine);
            DrawMap();
        }

        private void DrawStatsTable()
        {
            Console.ResetColor();
            Console.Write("Score:");
            foreach (var score in _game.Scores)
            {
                Console.Write('\t');
                Console.BackgroundColor = GetTeamColor(score.Key);
                Console.Write("{0}: {1}", score.Key, score.Value);
                Console.ResetColor();
            }
            Console.WriteLine();
            Console.BackgroundColor = GetTeamColor(_game.CurrentTeamId);
            Console.Write("TurnNo: {0}", _game.TurnNo);
            Console.ResetColor();

            Console.WriteLine();
        }

        private void DrawMap()
        {
            for (int y = 0; y < Board.Size; y++)
            {
                for (int x = 0; x < Board.Size; x++)
                {
                    string symbol = ".";
                    
                    Tile tile = _board.Map[x, y];

                    ConsoleColor background = ConsoleColor.Black;
                    ConsoleColor foreground = ConsoleColor.White;

                    switch (tile.Type)
                    {
                        case TileType.Unknown:
                        {
                            background = ConsoleColor.Gray;
                            break;
                        }
                        case TileType.Water:
                        {
                            background = ConsoleColor.Cyan;
                            break;
                        }
						case TileType.Chest1:
						{
							symbol = " ";
							background = ConsoleColor.Yellow;
							break;
						}
                        case TileType.Grass:
                        {
                            symbol = " ";
                            background = ConsoleColor.Green;
                            break;
                        }
                    }

                    if (tile.Coins > 0)
                    {
                        background = ConsoleColor.DarkYellow;
                        foreground = ConsoleColor.Yellow;
                        symbol = tile.Coins.ToString();
                    }

                    foreach (var team in _board.Teams)
                    {
                        var position = new Position(x, y);

                        var ship = team.Ship;
                        if (ship.Position == position)
                        {
                            foreground = ConsoleColor.White;
                            background = GetTeamColor(team.Id);
                            int crewCount = ship.Crew.Count;
                            symbol = crewCount == 0 ? "S" : crewCount.ToString();
                        }
                        else
                        {
                            Pirate pirate = tile.Pirates.FirstOrDefault();
                            if (pirate != null)
                            {
                                foreground = ConsoleColor.White;
                                background = GetTeamColor(team.Id);
                                //int coins = pirate.Coins;
                                //if (coins > 0)
                                //{
                                //    ;
                                //}
                                symbol = "p";//coins == 0 ? "p" : coins.ToString();
                            }
                        }
                    }
                    Console.BackgroundColor = background;
                    Console.ForegroundColor = foreground;
                    Console.Write(symbol);
                }
                Console.WriteLine();
            }
        }

        private static ConsoleColor GetTeamColor(int teamId)
        {
            switch (teamId)
            {
                case 0: return ConsoleColor.DarkRed;
                case 1: return ConsoleColor.DarkBlue;
                case 2: return ConsoleColor.DarkGreen;
                case 3: return ConsoleColor.DarkYellow;
                default: throw new NotSupportedException();
            }
        }

        public void GameOver()
        {
            Console.ResetColor();
            Console.Clear();
            Console.WriteLine("Game over");
            DrawStatsTable();
        }
    }
}