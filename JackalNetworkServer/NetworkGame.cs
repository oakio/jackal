using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Jackal;
using Jackal.GameOrganizer;
using JackalNetwork;

namespace JackalNetworkServer
{
    public class NetworkGame
    {
        private volatile bool  IsTerminated = false;

        public void Stop()
        {
            IsTerminated = true;
        }

        public void Start(List<ClientInfo> ConnectedClients, Action<string> loger, Action<string> resultReport)
        {
            try
            {
                while (IsTerminated == false)
                {
                    List<ClientInfo> playerList = null;
                    try
                    {
                        //выбираем клиентов
                        playerList = SelectPlayers(ConnectedClients);

                        if (playerList.Count < 2)
                        {
                            Thread.Sleep(TimeSpan.FromMilliseconds(10));
                            continue;
                        }

                        List<IPlayer> players = playerList.ConvertAll<IPlayer>(x => new NetworkPlayer(x));
                        while (players.Count < 4)
                            players.Add(new SmartPlayer2());

                        List<IdentifiedPlayer> identifiedPlayers = new List<IdentifiedPlayer>();
                        foreach (var client in playerList)
                        {
                            var identifiedPlayer = new IdentifiedPlayer(new NetworkPlayer(client), client.ClientId);
                            identifiedPlayers.Add(identifiedPlayer);
                        }
                        while (identifiedPlayers.Count < 4)
                        {
                            var player = new SmartPlayer2();
                            identifiedPlayers.Add(new IdentifiedPlayer(player, player.GetType().Name));
                        }

                        loger("Начался новый раунд, участники:\r\n" + string.Join("\r\n", identifiedPlayers.ConvertAll(x => x.Id)));

                        GameOrganizer organizer = new GameOrganizer(null, identifiedPlayers);
                        while (IsTerminated == false)
                        {
                            organizer.PlayNextGame();
                            string message = string.Format("Число игр {0}\r\n", organizer.Results.GamesCount);
                            message += string.Format("Текущая карта {0}/{1}\r\n", organizer.Results.MapId, organizer.Results.PermutationId);
                            message += organizer.Results.GetState();
                            resultReport(message);
                        }
                    }
                    catch (Exception exp)
                    {
                        loger("Error: " + exp.Message);
                    }
                    finally
                    {
                        if (playerList != null)
                        {
                            foreach (ClientInfo client in playerList)
                            {
                                client.InGame = false;
                            }
                        }
                    }

                    for (int i = 1; i <= 50; i++)
                    {
                        if (IsTerminated) break;
                        Thread.Sleep(TimeSpan.FromSeconds(0.1));
                    }
                }
            }
            catch (Exception exp)
            {
                loger(exp.Message);
            }
        }

        private static List<ClientInfo> SelectPlayers(List<ClientInfo> ConnectedClients)
        {
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
            return list;
        }

    }
}