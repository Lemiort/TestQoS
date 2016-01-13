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
    /// <summary>
    /// Основное окно программы
    /// </summary>
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

        /// <summary>
        /// Среднее значение целевой функции
        /// </summary>
        TrafficPlotter objectiveFunction;

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
            // Инициализация графиков
            inputTraffic = new TrafficPlotter("Входящий трафик");
            bucketMiss = new TrafficPlotter("Потери на маркерных корзинах");
            bucketGoal = new TrafficPlotter("Прошедший через маркерные корзины трафик");
            multiplexerMiss = new TrafficPlotter("Потери в мультиплексоре");
            multiplexerGoal = new TrafficPlotter("Прошедший через мультиплексор трафик");
            averageThroughput = new TrafficPlotter("Средняя пропускная способность");
            objectiveFunction = new TrafficPlotter("Значение целевой функции");
            objectiveFunction.AxisYTitle = "";
            // добавление в панель
            graphsTable1.Controls.Add(inputTraffic);
            graphsTable1.Controls.Add(bucketMiss);
            graphsTable1.Controls.Add(bucketGoal);
            graphsTable1.Controls.Add(multiplexerMiss);
            graphsTable2.Controls.Add(multiplexerGoal);
            graphsTable2.Controls.Add(averageThroughput);
            graphsTable2.Controls.Add(objectiveFunction);
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
                // применение настроек
                progressBar1.Visible = true;
                stopButton.Visible = true;
                timer1.Stop();

                //инициализируем
                qos = setForm.QoS;
                if (qos is SimpleTBQoS)
                {
                    (qos as SimpleTBQoS).Initializate(setForm.ObservationPeriod,
                                   setForm.NumOfBuckets,
                                   setForm.MinPacketSizes,
                                   setForm.MaxPacketSizes,
                                   setForm.MaxTimePeriods,
                                   setForm.MaxTimePeriods,
                                   setForm.HistorySize,
                                   setForm.MaxTokensCounts,
                                   setForm.Seed);
                    (qos as SimpleTBQoS).SetMultiplexer(setForm.MultiplexorBytesPerDt,
                                                        setForm.MultiplexorMaxQueueSize);
                    (qos as SimpleTBQoS).MultiplexerWeight = setForm.MultiplexerWeight;
                    (qos as SimpleTBQoS).QueueWeight = setForm.QueueWeight;
                    (qos as SimpleTBQoS).TokenBuketsWeights = setForm.TokenBuketsWeights;
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
                objectiveFunction.Clear();

                for (int j = 0; j < ((qos as SimpleTBQoS).bucketsAnalyzer as SimpleAnalyzer).quantsPassed.Count; j++)
                {
                    //потери на вёдрах
                    uint bucketMissValue = (uint)((qos as SimpleTBQoS).bucketsAnalyzer as SimpleAnalyzer).quantsNotPassed.ElementAt(j).SummarySize;
                   /* for (int i = 0; i < (qos as SimpleTBQoS).bucketAnalyzers.Count; i++)
                    {
                        bucketMissValue += (uint)((qos as SimpleTBQoS).bucketAnalyzers[i] as SimpleAnalyzer).GetAverageNotPassedPacketsSize();
                    }*/

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

                    //среднее значение целевой функции
                    float averageObjectiveFunctionValue = ((qos as SimpleTBQoS).objectiveFunctionHistory.ElementAt(j));
                    objectiveFunction.AddPoint(averageObjectiveFunctionValue, (float)j);
                }
            }
            if (backgroundWorker1.IsBusy == false)
                backgroundWorker1.RunWorkerAsync();
        }
    }
}
