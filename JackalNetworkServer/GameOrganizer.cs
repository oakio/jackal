using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Jackal;
using JackalNetwork;

namespace JackalNetworkServer
{
    public class Results
    {
        Dictionary<string,List<int>> table=new Dictionary<string, List<int>>();

        public string GetState()
        {
            String rez = "";
            lock (table)
            {
                foreach (var pair in  table.OrderBy(x => x.Value.Average(y => y)))
                {
                    rez += pair.Key + " " + pair.Value.Average(y => y)+"\r\n";
                }
            }
            return rez;
        }

        public void AddGameResult(string clientId, int position)
        {
            lock (table)
            {
                List<int> list;
                if (table.TryGetValue(clientId, out list) == false)
                {
                    list = new List<int>();
                    table[clientId] = list;
                }
                list.Add(position);
            }
        }
    }

    public class GameOrganizer
    {
        private bool IsTerminated = false;

        public void Do(List<ClientInfo> ConnectedClients, Action<string> loger, Action<string> resultReport)
        {
            Results results=new Results();

            try
            {
                int rountId = 0;

                while (IsTerminated == false)
                {
                    rountId++;

                    List<ClientInfo> list = new List<ClientInfo>();
                    lock (ConnectedClients)
                    {
                        foreach (ClientInfo client in ConnectedClients.OrderBy(x => Guid.NewGuid()))
                        {
                            if (client.InGame == false)
                                list.Add(client);
                            if (list.Count >= 4) break;
                        }
                        if (list.Count >= 2)
                        {
                            foreach (ClientInfo client in list)
                            {
                                client.InGame = true;
                            }
                        }
                    }

                    if (list.Count < 2)
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(10));
                        continue;
                    }

                    List<IPlayer> players = list.ConvertAll<IPlayer>(x => new NetworkPlayer(x));
                    while (players.Count < 4)
                        players.Add(new SmartPlayer());


                     int mapId = 987412 + rountId;

                    loger("New game begin: " + string.Join(",", list.ConvertAll(x => x.ClientId)));

                    var board = new Board(mapId);
                    var game = new Game(players.ToArray(), board);

                    try
                    {
                        while (game.IsGameOver == false && IsTerminated == false)
                        {
                            loger("Game: turn " + game.TurnNo);
                            //Console.ReadKey();
                            game.Turn();
                        }

                        loger("Game end.");

                        GameReport gameReport = CreateGameReport(game);

                        gameReport.GamersId = list.ConvertAll(x => x.ClientId).ToArray();
                        gameReport.GamersStanding = GetStanding(game);

                        for (int id = 0; id < list.Count; id++)
                        {
                            results.AddGameResult(gameReport.GamersId[id], gameReport.GamersStanding[id]);
                        }

                        resultReport(results.GetState());

                        foreach (var client in list)
                        {
                            client.Query(gameReport);
                        }
                    }
                    catch (Exception exp)
                    {
                        loger("Game failed: "+exp.Message);
                    }

                    foreach (ClientInfo client in list)
                    {
                        client.InGame = false;
                    }
                }
            }
            catch (Exception exp)
            {
                loger(exp.Message);
            }
        }

        private int[] GetStanding(Game game)
        {
            List<int> gold = new List<int>(4);
            for (int id = 0; id <= 3; id++)
            {
                gold.Add( game.Scores[id]);
            }

            //определяем место
            List<int> list = new List<int>(4);
            for (int id = 0; id <= 3; id++)
            {
                int currentGold = gold[id];
                list.Add( 1 + gold.Count(x => x > currentGold));
            }

            return list.ToArray();
        }

        private GameReport CreateGameReport(Game game)
        {
           GameReport rez=new GameReport();

            rez.IsGameOver = game.IsGameOver;

            rez.GameId = game.GameId;
            rez.GamersId = new string[4];
            rez.IsGameOver = game.IsGameOver;
            GameState gameState=new GameState();
            gameState.AvailableMoves = new Move[] {};
            gameState.Board = game.Board;
            gameState.GameId = game.GameId;
            rez.State = gameState;
            return rez;
        }
    }
}