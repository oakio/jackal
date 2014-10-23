using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Jackal;
using JackalNetwork;
using Newtonsoft.Json;


namespace JackalNetworkClient
{
    public partial class Form1 : Form
    {
        private IPlayer player = new SmartPlayer();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var serverUri = new Uri(textBoxServer.Text);

            Thread connectionThread = new Thread(() => DoConnection(serverUri, Log));
            connectionThread.Start();
        }

        private void DoConnection(Uri serverUri,Action<string> reporter)
        {
            while (true)
            {
                try
                {
                    TcpClient client = new TcpClient();
                    client.Connect(serverUri.Host, serverUri.Port);
                    NetworkStream stream = client.GetStream();

                    reporter("Connected");

                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    writer = new StreamWriter(stream, Encoding.UTF8);

                    while (client.Connected)
                    {
                        string line = reader.ReadLine();
                        NetworkMessage message = JsonHelper.DeserialiazeWithType<NetworkMessage>(line);
                        reporter("Received message: " + message.GetType() + "\r\n");
                        ProcessMessage(message);
                    }

                    // Close everything.
                    writer.Close();
                    client.Close();
                    stream.Close();
                    client.Close();
                }
                catch (Exception exp)
                {
                    reporter("Error: " + exp.Message + "\r\n");
                }
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }

        private StreamWriter writer;

        void ProcessMessage(NetworkMessage message)
        {
            if (message is CheckRequest)
                ProcessCheckRequest(message as CheckRequest);
            else if (message is DecisionRequest)
                ProcessDecisionRequest(message as DecisionRequest);
            else if (message is WelcomeRequest)
                ProcessWelcomeRequest(message as WelcomeRequest);
        }


        private void SendAnswer(NetworkMessage message)
        {
            var str = JsonHelper.SerialiazeWithType(message);
            writer.WriteLine(str);
            writer.Flush();
        }

        private void ProcessWelcomeRequest(WelcomeRequest welcomeRequest)
        {
            var welcomeAnswer = new WelcomeAnswer();
            welcomeAnswer.ClientName = player.GetType().Name.ToString() + "_"+Guid.NewGuid();
            SendAnswer(welcomeAnswer);
        }

        private void ProcessDecisionRequest(DecisionRequest decisionRequest)
        {
            var decisionAnswer = new DecisionAnswer();
            decisionAnswer.RequestId = decisionRequest.RequestId;
            decisionAnswer.Decision = player.OnMove(decisionRequest.State);
            SendAnswer(decisionAnswer);
        }

        private void ProcessCheckRequest(CheckRequest checkRequest)
        {
            var сheckAnswer = new CheckAnswer();
            SendAnswer(сheckAnswer);
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


    }
}
