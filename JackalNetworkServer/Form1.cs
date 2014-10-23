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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GameOrganizer organizer = new GameOrganizer();
            Thread organizerThread = new Thread(() => organizer.Do(ConnectedClients, Log,ReportState));
            organizerThread.Start();
            Thread listenerThread = new Thread(StartListener);
            listenerThread.Start();
        }

        private void ReportState(string s)
        {
            UpdateUI(() => textBoxReport.Text = s);
        }

        void StartListener()
        {
            var server = System.Net.Sockets.TcpListener.Create(29998);
            server.Start();

            // Enter the listening loop. 
            while (true)
            {
                Log("Waiting for a connection...");

                // Perform a blocking call to accept requests. 
                // You could also user server.AcceptSocket() here.
                TcpClient client = server.AcceptTcpClient();

                Thread thread = new Thread(ProcessConnection);
                thread.Start(client);
            }
        }

        void Log(string line)
        {
            UpdateUI(() => textBoxLog.AppendText(line + "\r\n"));
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

                lock (ConnectedClients)
                {
                    ConnectedClients.Add(client);
                }

                WelcomeRequest welcome = new WelcomeRequest();
                welcome.ServerId = serverId;
                welcome.DecisionTimeout = 60;

                var answer=client.Query(welcome) as WelcomeAnswer;
                client.ClientId = answer.ClientName;

                while (tcpClient.Connected)
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
    }
}
