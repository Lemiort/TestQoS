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
        TestQoS.QoS qos;

        //
        // графики
        //

        /// <summary>
        /// график входного трафика
        /// </summary>
        TrafficPlotter inputTraffic;

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
        /// прошедший трафик
        /// </summary>
        TrafficPlotter multiplexerGoal;

        /// <summary>
        /// Средняя пропускная способность
        /// </summary>
        TrafficPlotter averageThroughput;


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
            inputTraffic = new TrafficPlotter("Входящий трафик");
            bucketMiss = new TrafficPlotter("Потери на маркерных корзинах");
            bucketGoal = new TrafficPlotter("Прошедший через маркерные корзины трафик");
            multiplexerMiss = new TrafficPlotter("Потери в мультиплексоре");
            multiplexerGoal = new TrafficPlotter("Прошедший через мультиплексор трафик");
            averageThroughput = new TrafficPlotter("Средняя пропускная способность");
            // добавляем в панели
            graphsTabel.Controls.Add(inputTraffic);   
            graphsTabel.Controls.Add(bucketMiss);
            graphsTabel.Controls.Add(bucketGoal);
            graphsTabel.Controls.Add(multiplexerMiss);
            graphsTabel.Controls.Add(multiplexerGoal);    
            graphsTabel.Controls.Add(averageThroughput);
        }

        /// <summary>
        /// применение настроек
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
   //             qos = new TestQoS.SimpleTBQoS();
                qos = setForm.QoS;
                if (qos is SimpleTBQoS)
                {
                    (qos as SimpleTBQoS).Initializate(setForm.ObservationPeriod(),
                                   setForm.NumOfBuckets(),
                                   setForm.MinPacketSizes(),
                                   setForm.MaxPacketSizes(),
                                   setForm.MaxTimePeriods(),
                                   setForm.MaxTimePeriods(),
                                   setForm.HistorySize());
                    //MessageBox.Show(setForm.GetMultiplexorBytesPerDt().ToString());
                    (qos as SimpleTBQoS).SetMultiplexerSpeed(setForm.GetMultiplexorBytesPerDt());
                }

                if (qos is SimulatedAnnealingQoS)
                {
                    (qos as SimulatedAnnealingQoS).MultiplexerWeight = setForm.MultiplexerWeight();
                    (qos as SimulatedAnnealingQoS).QueueWeight = setForm.QueueWeight();
                    (qos as SimulatedAnnealingQoS).TokenBuketsWeights = setForm.TokenBuketsWeights();
                }

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
             ((qos as SimpleTBQoS).multiplexorAnalyzer as SimpleAnalyzer).GetAverageNotPassedPacketsSize().ToString();
            label1.Text += "\nСреднее число пропущенных байтов в квант=  " +
              ((qos as SimpleTBQoS).multiplexorAnalyzer as SimpleAnalyzer).GetAveragePassedPacketsSize().ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Среднее число отброшенных байтов в квант=  " +
              ((qos as SimpleTBQoS).multiplexorAnalyzer as SimpleAnalyzer).GetAverageNotPassedPacketsSize().ToString();
            label1.Text += "\nСреднее число пропущенных байтов в квант=  " +
              ((qos as SimpleTBQoS).multiplexorAnalyzer as SimpleAnalyzer).GetAveragePassedPacketsSize().ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (backgroundWorker1.IsBusy)
                backgroundWorker1.CancelAsync();
            if (backgroundWorker1.IsBusy == false)
            {

                bucketMiss.Clear();
                bucketGoal.Clear();
                inputTraffic.Clear();
                multiplexerGoal.Clear();
                multiplexerMiss.Clear();
                averageThroughput.Clear();

                for (int j = 0; j < ((qos as SimpleTBQoS).bucketsAnalyzer as SimpleAnalyzer).quantsPassed.Count; j++)
                {
                    //потери на вёдрах
                    uint bucketMissValue = (uint)((qos as SimpleTBQoS).bucketsAnalyzer as SimpleAnalyzer).quantsNotPassed.ElementAt(j).SummarySize;
                    //прошедший траффик
                    uint bucketGoalValue = (uint)((qos as SimpleTBQoS).bucketsAnalyzer as SimpleAnalyzer).quantsPassed.ElementAt(j).SummarySize;
                    bucketMiss.AddPoint(bucketMissValue, (double)j);
                    bucketGoal.AddPoint(bucketGoalValue, (double)j);
                    inputTraffic.AddPoint(bucketMissValue + bucketGoalValue, (double)j);

                    //потери на мультиплексоре
                    uint multiplexorMissValue = (uint)((qos as SimpleTBQoS).multiplexorAnalyzer as SimpleAnalyzer).quantsNotPassed.ElementAt(j).SummarySize;
                    multiplexerMiss.AddPoint(multiplexorMissValue, (double)j);

                    //прошедший через мультиплексор траффик
                    uint multiplexorGoalValue = (uint)((qos as SimpleTBQoS).multiplexorAnalyzer as SimpleAnalyzer).quantsPassed.ElementAt(j).SummarySize;
                    multiplexerGoal.AddPoint(multiplexorGoalValue, (double)j);

                    //средняя пропускная способность
                    float averageThroghputValue = ((qos as SimpleTBQoS).multiplexorAverageBytes.ElementAt(j));
                    averageThroughput.AddPoint(averageThroghputValue, (float)j);
                }
            }
            if (backgroundWorker1.IsBusy == false)
                backgroundWorker1.RunWorkerAsync();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Тык!","",MessageBoxButtons.RetryCancel,MessageBoxIcon.Hand,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        }


    }
}
