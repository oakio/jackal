using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Jackal;
using Jackal.Players;
using JackalNetwork;
using Newtonsoft.Json;


namespace JackalNetworkClient
{
    public partial class Form1 : Form
    {
        private IPlayer player = new MikePlayer();

        public Form1()
        {
            InitializeComponent();
        }

        private volatile bool IsTerminated = false;

        private void button1_Click(object sender, EventArgs e)
        {
            var serverUri = new Uri(textBoxServer.Text);

            Thread connectionThread = new Thread(() => DoConnection(serverUri, Log));
            connectionThread.Start();
            button1.Enabled = false;
        }

        private void DoConnection(Uri serverUri,Action<string> reporter)
        {
            while (IsTerminated==false)
            {
                try
                {
                    TcpClient client = new TcpClient();
                    client.Connect(serverUri.Host, serverUri.Port);
                    NetworkStream stream = client.GetStream();

                    reporter("Connected");

                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

                    while (client.Connected && IsTerminated == false)
                    {
                        string line = reader.ReadLine();
                        NetworkCommunication message = JsonHelper.DeserialiazeWithType<NetworkCommunication>(line);
                        reporter("Received message: " + message.GetType());
                        var answer = ProcessMessage(message);
                        if (answer != null)
                        {
                            var str = JsonHelper.SerialiazeWithType(answer);
                            writer.WriteLine(str);
                            writer.Flush();
                        }
                    }

                    // Close everything.
                    reader.Close();
                    writer.Close();
                    stream.Close();
                    client.Close();
                }
                catch (Exception exp)
                {
                    reporter("Error: " + exp.Message);
                }
                for (int i = 1; i <= 50; i++)
                {
                    if (IsTerminated) break;
                    Thread.Sleep(TimeSpan.FromSeconds(0.1));
                }
            }
        }



        NetworkMessage ProcessMessage(NetworkCommunication communication)
        {
            if (communication is CheckRequest)
                return ProcessCheck(communication as CheckRequest);
            else if (communication is NewGameMessage)
                ProcessNewGame(communication as NewGameMessage);
            else if (communication is DecisionRequest)
                return ProcessDecision(communication as DecisionRequest);
            else if (communication is WelcomeRequest)
                return ProcessWelcome(communication as WelcomeRequest);
            return null;
        }

        private void ProcessNewGame(NewGameMessage newGameMessage)
        {
            player.OnNewGame();
        }

        private WelcomeAnswer ProcessWelcome(WelcomeRequest welcomeRequest)
        {
            var welcomeAnswer = new WelcomeAnswer();
            welcomeAnswer.ClientName = player.GetType().Name.ToString() + "_" + Dns.GetHostName();
            return welcomeAnswer;
        }

        private DecisionAnswer ProcessDecision(DecisionRequest decisionRequest)
        {
            var decisionAnswer = new DecisionAnswer();
            decisionAnswer.RequestId = decisionRequest.RequestId;
            var gameState = decisionRequest.GetGameState();
            decisionAnswer.Decision = player.OnMove(gameState);
            return decisionAnswer;
        }

        private CheckAnswer ProcessCheck(CheckRequest checkRequest)
        {
            var сheckAnswer = new CheckAnswer();
            return сheckAnswer;
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

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsTerminated = true;
        }


    }
}
