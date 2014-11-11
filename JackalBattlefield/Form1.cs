using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Jackal;
using Jackal.GameOrganizer;
using Jackal.Players;

namespace JackalBattlefield
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private volatile bool IsStarted = false;
        private volatile bool IsStopped = false;

        private Thread thread;

        Stopwatch stopwatch=new Stopwatch();

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsStarted == false)
            {
                IsStarted = true;
                IsStopped = false;

                stopwatch.Reset();
                stopwatch.Start();
                thread = new Thread(DoBattlefield);
                thread.Start();

                button1.Text = "Остановка";
                timer1.Start();
            }
            else
            {
                IsStarted = false;
                IsStopped = true;
                button1.Text = "Запуск";
                timer1.Stop();
                thread.Join();
            }
        }

        private GameOrganizer gameOrganizer;

		private IPlayer[] _players = new IPlayer[4] { new SmartPlayer(), new SmartPlayer2(), new SmartPlayer2(), new SmartPlayerDistCalc() };

        void DoBattlefield()
        {
            try
            {
                const int mapId = 1;
                var identifiedPlayers = _players.Select(x => new IdentifiedPlayer(x, x.GetType().Name));
                gameOrganizer = new GameOrganizer(mapId, identifiedPlayers);

                while (IsStopped == false)
                {
                    gameOrganizer.PlayNextGame();
                }
            }
            catch (Exception exp)
            {
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var results = gameOrganizer.Results;

            double seconds = stopwatch.Elapsed.TotalSeconds;
            if (seconds < 1)
                seconds = 1;
            label1.Text = string.Format("Число игр: {0}, время: {1} с, игр в секунду: {2}, текущая карта {3}/{4}",
                results.GamesCount,
                seconds.ToString("N0"),
                (1.0 * results.GamesCount / seconds).ToString("N0"),
                results.MapId,
                results.PermutationId);
            textBox1.Text = results.GetState();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsStopped = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "";
        }
    }



}
