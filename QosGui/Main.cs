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
            //TODO: настройки по умолчанию
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

                //инициализируем
                qos = new TestQoS.SimpleTBQoS();
                qos.Initializate(setForm.ObservationPeriod(),
                                setForm.NumOfBuckets(),
                                setForm.MinPacketSizes(),
                                setForm.MaxPacketSizes(),
                                setForm.MaxTimePeriods(),
                                setForm.MaxTimePeriods());
                if (! backgroundWorker1.IsBusy)
                    backgroundWorker1.RunWorkerAsync();
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

            progressBar1.Visible = false;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label1.Text = "Среднее число отброшенных пакетов в квант=  " +
             (qos.analyzer as SimpleAnalyzer).GetAverageNotPassedPacketsSize().ToString();
            label1.Text += "\nСреднее число пропущенных пакетов в квант=  " +
              (qos.analyzer as SimpleAnalyzer).GetAveragePassedPacketsSize().ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Среднее число отброшенных пакетов в квант=  " +
              (qos.analyzer as SimpleAnalyzer).GetAverageNotPassedPacketsSize().ToString();
            label1.Text += "\nСреднее число пропущенных пакетов в квант=  " +
              (qos.analyzer as SimpleAnalyzer).GetAveragePassedPacketsSize().ToString();
        }


    }
}
