using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestQoS;

namespace QosGui
{
    public partial class Main : Form
    {
        /// <summary>
        /// диалог настроек
        /// </summary>
        Settings setForm;

        /// <summary>
        /// сама модель
        /// </summary>
        TestQoS.SimpleTBQoS qos;

        //
        // графики
        //

        /// <summary>
        /// Потери на вёдрах
        /// </summary>
        TrafficPlotter bucketMiss;

        /// <summary>
        /// Прошедний через ведро
        /// </summary>
        TrafficPlotter bucketGoal;

        /// <summary>
        /// потери на мультиплексоре
        /// </summary>
        TrafficPlotter multiplexerMiss;

        /// <summary>
        /// прошедший через мультиплексор
        /// </summary>
        TrafficPlotter multiplexerGoal;



        Random rand;

        public Main()
        {
            InitializeComponent();
            progressBar1.Visible = false;
            stopButton.Visible = false;

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;

            rand = new Random();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //TODO: назвать это всё вменяемо
            bucketMiss = new TrafficPlotter("Потери на вёдрах");
            bucketGoal = new TrafficPlotter("Прошедши через вёдра трафик");
            multiplexerMiss = new TrafficPlotter("Потери в мультиплексоре");
            multiplexerGoal = new TrafficPlotter("Прошедши через мультиплексор трафик");

            // добавляем в панели
            graphsTabel.Controls.Add(bucketMiss);           
            graphsTabel.Controls.Add(multiplexerMiss);
            graphsTabel.Controls.Add(bucketGoal);
            graphsTabel.Controls.Add(multiplexerGoal);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // открываем меню настроек
            if(setForm == null)
                setForm = new Settings();

            setForm.ShowDialog();
            
            if(setForm.IsSettingsApplyed)
            {
                if(backgroundWorker1.IsBusy)
                    backgroundWorker1.CancelAsync();
                // где-то тут применяются настройки
                progressBar1.Visible = true;
                stopButton.Visible = true;
                label1.Visible = true;
                timer1.Stop();

                //инициализируем
                qos = new TestQoS.SimpleTBQoS();
                qos.Initializate(setForm.ObservationPeriod(),
                                setForm.NumOfBuckets(),
                                setForm.MinPacketSizes(),
                                setForm.MaxPacketSizes(),
                                setForm.MaxTimePeriods(),
                                setForm.MaxTimePeriods());
                qos.SetMultiplexerSpeed(setForm.GetMultiplexorBytesPerDt());
                if (!backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.RunWorkerAsync();
                    timer1.Start();
                }
                else
                {
                    MessageBox.Show("Background worker is busy!");
                }
            }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            //выполняем один такт
            while(worker.CancellationPending == false)
            {
                qos.MakeTick();
               // worker.ReportProgress(rand.Next(1, 90));
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {

            stopButton.Visible = false;
            if (backgroundWorker1.IsBusy)
                backgroundWorker1.CancelAsync();
            timer1.Stop();
            progressBar1.Visible = false;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label1.Text = "Среднее число отброшенных байтов в квант=  " +
             (qos.analyzer as SimpleAnalyzer).GetAverageNotPassedPacketsSize().ToString();
            label1.Text += "\nСреднее число пропущенных байтов в квант=  " +
              (qos.analyzer as SimpleAnalyzer).GetAveragePassedPacketsSize().ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Среднее число отброшенных байтов в квант=  " +
              (qos.analyzer as SimpleAnalyzer).GetAverageNotPassedPacketsSize().ToString();
            label1.Text += "\nСреднее число пропущенных байтов в квант=  " +
              (qos.analyzer as SimpleAnalyzer).GetAveragePassedPacketsSize().ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = "Среднее число отброшенных байтов в квант=  " +
              (qos.analyzer as SimpleAnalyzer).GetAverageNotPassedPacketsSize().ToString();
            label1.Text += "\nСреднее число пропущенных байтов в квант=  " +
              (qos.analyzer as SimpleAnalyzer).GetAveragePassedPacketsSize().ToString();

            if (backgroundWorker1.IsBusy)
                backgroundWorker1.CancelAsync();

            bucketMiss.Clear();
            bucketGoal.Clear();
            multiplexerGoal.Clear();
            multiplexerMiss.Clear();

            int i=0;
            foreach(TestQoS.HistoryQuant quant in (qos.analyzer as SimpleAnalyzer).quantsNotPassedBucket)
            {
                bucketMiss.AddPoint((uint)quant.summarySize, (double)i++);
            }
            i = 0;
            foreach (TestQoS.HistoryQuant quant in (qos.analyzer as SimpleAnalyzer).quantsPassedBucket)
            {
                bucketGoal.AddPoint((uint)quant.summarySize, (double)i++);
            }
            i = 0;
            foreach (TestQoS.HistoryQuant quant in (qos.analyzer as SimpleAnalyzer).quantsNotPassedMultiplexer)
            {
                multiplexerMiss.AddPoint((uint)quant.summarySize, (double)i++);
            }
            i = 0;
            foreach (TestQoS.HistoryQuant quant in (qos.analyzer as SimpleAnalyzer).quantsPassedMultiplexer)
            {
                multiplexerGoal.AddPoint((uint)quant.summarySize, (double)i++);
            }
            if (!backgroundWorker1.IsBusy)
                backgroundWorker1.RunWorkerAsync();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Тык!","",MessageBoxButtons.RetryCancel,MessageBoxIcon.Hand,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        }


    }
}
