using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using JackalNetwork;

namespace JackalNetworkServer
{
    public partial class Form1 : Form
    {
        private volatile bool IsTerminated = false;

        public Form1()
        {
            InitializeComponent();
        }

        private NetworkGame networkGame;

        private void Form1_Load(object sender, EventArgs e)
        {
            networkGame = new NetworkGame();
            Thread organizerThread = new Thread(() => networkGame.Start(ConnectedClients, Log, ReportState));
            organizerThread.Start();
            Thread listenerThread = new Thread(StartListener);
            listenerThread.Start();
        }

        private void ReportState(string s)
        {
            try
            {
                UpdateUI(() => textBoxReport.Text = s);
            }
            catch (Exception exp)
            {
            }
        }

        void StartListener()
        {
            try
            {
                var server = System.Net.Sockets.TcpListener.Create(29998);
                server.Start();

                Log("Started listening...");

                // Enter the listening loop. 
                while (IsTerminated == false)
                {
                    if (server.Pending() == false)
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(10));
                        continue;
                    }

                    // Perform a blocking call to accept requests. 
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();

                    Thread thread = new Thread(ProcessConnection);
                    thread.Start(client);
                }
            }
            catch (Exception exp)
            {
                Log("Error: " + exp.Message);
            }
        }

        void Log(string line)
        {
            try
            {
                UpdateUI(() => textBoxLog.Text = line);
            }
            catch (Exception exp)
            {
            }
        }

        /// <summary>
        /// Вызываем процедуру обновления
        /// </summary>
        /// <param name="action"></param>
        void UpdateUI(Action action)
        {
            UpdateUI<object>(x => action(), null);
        }

        /// <summary>
        /// Вызываем процедуру обновления
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="data"></param>
        void UpdateUI<T>(Action<T> action, T data)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<T>((t) =>
                {
                    try
                    {
                        action(t);
                    }
                    catch (Exception ex)
                    {
                        //Logger.Error("UpdateUI() failed", ex);
                    }
                }), new object[] { data });
            }
            else
            {
                action(data);
            }
        }



        List<ClientInfo> ConnectedClients=new List<ClientInfo>();

        private void ProcessConnection(object param)
        {
            try
            {
                TcpClient tcpClient = param as TcpClient;

                Log("Connected: "+tcpClient.Client.RemoteEndPoint);

                // Get a stream object for reading and writing
                NetworkStream stream = tcpClient.GetStream();

                ClientInfo client = new ClientInfo();

                client.reader = new StreamReader(stream, Encoding.UTF8);
                client.writer = new StreamWriter(stream, Encoding.UTF8);
                client.writer.AutoFlush = true;

                client.TcpClient = tcpClient;

                WelcomeRequest welcome = new WelcomeRequest();
                welcome.ServerId = serverId;
                welcome.DecisionTimeout = 60;

                var answer=client.Query(welcome) as WelcomeAnswer;
                client.ClientId = answer.ClientName;

                lock (ConnectedClients)
                {
                    ConnectedClients.Add(client);
                }

                while (tcpClient.Connected && IsTerminated == false)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(10));
                }

                Log("Disconnected");

                lock (ConnectedClients)
                {
                    ConnectedClients.Remove(client);
                }
            }
            catch (Exception exp)
            {
                Log("Error: " + exp.Message);
            }
        }

        private Guid serverId = Guid.NewGuid();

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            networkGame.Stop();
            IsTerminated = true;
        }
    }
}
