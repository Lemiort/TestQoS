﻿using System;
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
    /// Окно настроек системы
    /// Включает в себя настройки количества потоков, интервала наблюдения.
    /// Также в отдельных вкладках нахотятся настройки для каждого потока
    /// </summary>
    public partial class Settings : Form
    {
        /// <summary>
        /// Хранит модель QoS, которой мы хотим всё инициализировать
        /// </summary>
        public QoS QoS { get; private set; }

        /// <summary>
        /// возращает истину, если были применены новые параметры
        /// </summary>
        public bool IsSettingsApplyed { get; private set; }

        /// <summary>
        /// список хранит минимальные размеры пакетов 
        /// </summary>
        private List<NumericUpDown> minPacketSizes;

        /// <summary>
        /// список хранит максимальные размеры пакетов
        /// </summary>
        private List<NumericUpDown> maxPacketSizes;

        /// <summary>
        /// список хранит минимальные промежутоки времени между двумя пакетами в милисекундах
        /// </summary>
        private List<NumericUpDown> minTimePeriods;

        /// <summary>
        /// список хранит максимальные промежутоки времени между двумя пакетами в милисекундах
        /// </summary>
        private List<NumericUpDown> maxTimePeriods;

        /// <summary>
        /// хранит размеры маркерных корзин
        /// </summary>
        private List<NumericUpDown> maxTokensCounts;

        /// <summary>
        /// Веса маркерных корзин
        /// </summary>
        private List<TrackBar> tokenBuketWeights;


        /// <summary>
        /// период наблюдения системы
        /// </summary>
        /// <returns></returns>
        public double ObservationPeriod
        {
            get
            {
                return (double)observationPeriod.Value;
            }
        }

        /// <summary>
        /// кол-во потоков/вёдер
        /// </summary>
        /// <returns></returns>
        public uint NumOfBuckets
        {
            get
            {
                return (uint)bucketNum.Value;
            }
        }

        /// <summary>
        /// Минимальный размер пакета
        /// </summary>
        /// <returns></returns>
        public List<uint> MinPacketSizes
        {
            get
            {
                List<uint> result = new List<uint>();
                foreach (var minPacketSize in minPacketSizes)
                {
                    result.Add((uint)minPacketSize.Value);
                }
                return result;
            }
        }

        /// <summary>
        /// Максимальный размер пакета
        /// </summary>
        /// <returns></returns>
        public List<uint> MaxPacketSizes
        {
            get
            {
                List<uint> result = new List<uint>();
                foreach (var maxPacketSize in maxPacketSizes)
                {
                    result.Add((uint)maxPacketSize.Value);
                }
                return result;
            }
        }

        /// <summary>
        /// Размер маркерных корзин
        /// </summary>
        /// <returns></returns>
        public List<float> MaxTokensCounts
        {
            get
            {
                List<float> result = new List<float>();
                foreach (var maxTokensCount in maxTokensCounts)
                {
                    result.Add((float)maxTokensCount.Value);
                }
                return result;
            }
        }

        /// <summary>
        /// Минимальный промежуток времени между двумя пакетами в милисекундах
        /// </summary>
        /// <returns></returns>
        public List<double> MinTimePeriods
        {
            get
            {
                List<double> result = new List<double>();
                foreach (var minTimePeriod in minTimePeriods)
                {
                    result.Add((double)minTimePeriod.Value);
                }
                return result;
            }
        }

        /// <summary>
        /// Максимальный промежуток времени между двумя пакетами в милисекундах
        /// </summary>
        /// <returns></returns>
        public List<double> MaxTimePeriods
        {
            get
            {
                List<double> result = new List<double>();
                foreach (var maxTimePeriod in maxTimePeriods)
                {
                    result.Add((double)maxTimePeriod.Value);
                }
                return result;
            }
        }

        /// <summary>
        /// Вес потерь на мультиплексоре
        /// </summary>
        /// <returns></returns>
        public uint MultiplexerWeight
        {
            get
            {
                return (uint)multiplexerWeight.Value;
            }
        }

        /// <summary>
        /// Вес очереди мультиплексора
        /// </summary>
        /// <returns></returns>
        public uint QueueWeight
        {
            get
            {
                return (uint)queueWeight.Value;
            }
        }

        /// <summary>
        /// Вес потерь на корзинах
        /// </summary>
        /// <returns></returns>
        public List<uint> TokenBuketsWeights
        {
            get
            {
                List<uint> result = new List<uint>();
                foreach (var tokenBuketWeight in tokenBuketWeights)
                {
                    result.Add((uint)tokenBuketWeight.Value);
                }
                return result;
            }
        }

        /// <summary>
        /// возвращает величину истории
        /// </summary>
        /// <returns>длина в квантах</returns>
        public int HistorySize
        {
            get
            {
                return (int)historySize.Value;
            }
        }

        /// <summary>
        /// Ширина пропускания канала
        /// </summary>
        /// <returns></returns>
        public ulong MultiplexorBytesPerDt
        {
            get
            {
                return Decimal.ToUInt64(multiplexerSpeed.Value * observationPeriod.Value);
            }
        }

        /// <summary>
        /// Длинна очереди мультиплексора
        /// </summary>
        public ulong MultiplexorMaxQueueSize
        {
            get
            {
                return Decimal.ToUInt64(queueLength.Value);
            }
        }

        /// <summary>
        /// Сид генератора трафика
        /// </summary>
        public int Seed
        {
            get
            {
                return Decimal.ToInt32(seed.Value);
            }
        }

        public Settings()
        {
            // инициализация окна настроек
            InitializeComponent();
            this.QoS = new AverageStrategyQos();
            this.minPacketSizes = new List<NumericUpDown>();
            this.maxPacketSizes = new List<NumericUpDown>();
            this.minTimePeriods = new List<NumericUpDown>();
            this.maxTimePeriods = new List<NumericUpDown>();
            this.maxTokensCounts = new List<NumericUpDown>();
            this.tokenBuketWeights = new List<TrackBar>();
            InitGeneratorSettings();
            this.IsSettingsApplyed = false;
        }

        /// <summary>
        /// Инициализация объекта generatorSettings
        /// // TODO: поиграться с константами
        /// </summary>
        private void InitGeneratorSettings()
        {
            // максимальный период
            int periodMax = 10000;
            // максимальный размер
            int sizeMax = 10000;
            // шаг периода
            int periodStep = 10;
            // шаг размера
            int sizeStep = 100;

            // подготовка к инициализации
            this.generatorsSettings.TabPages.Clear();
            minPacketSizes.Clear();
            maxPacketSizes.Clear();
            minTimePeriods.Clear();
            maxTimePeriods.Clear();
            tokenBuketWeights.Clear();

            // инициализация
            int N = (int)bucketNum.Value;
            for(int i=0; i < N; i++)
            {
                // очередная страница
                TabPage tabPage = new TabPage((i+1).ToString());

                // создаём лейблы
                Label label1 = new Label();
                label1.AutoSize = true;
                label1.Location = new System.Drawing.Point(16, 18);
                label1.Size = new System.Drawing.Size(159, 13);
                label1.Text = "Минимальный размер пакета (байт)";

                Label label2 = new Label();
                label2.AutoSize = true;
                label2.Location = new System.Drawing.Point(16, 48);
                label2.Size = new System.Drawing.Size(165, 13);
                label2.Text = "Максимальный размер пакета (байт)";

                Label label3 = new Label();
                label3.AutoSize = true;
                label3.Location = new System.Drawing.Point(16, 91);
                label3.Size = new System.Drawing.Size(175, 13);
                label3.Text = "Минимальный период генерации (мс)";

                Label label4 = new Label();
                label4.AutoSize = true;
                label4.Location = new System.Drawing.Point(16, 123);
                label4.Size = new System.Drawing.Size(181, 13);
                label4.Text = "Максимальный период генерации (мс)";

                Label label5 = new Label();
                label5.AutoSize = true;
                label5.Location = new System.Drawing.Point(16, 162);
                label5.Size = new System.Drawing.Size(181, 13);
                label5.Text = "Размер корзины";

                Label label6 = new Label();
                label6.AutoSize = true;
                label6.Location = new System.Drawing.Point(16, 200);
                label6.Size = new System.Drawing.Size(181, 13);
                label6.Text = "Вес потерь на корзине";

                // создаём нумерики
                // Минимальный размер пакета
                NumericUpDown minPacketSize = new NumericUpDown();
                minPacketSize.Location = new System.Drawing.Point(232, 16);
                minPacketSize.Maximum = new decimal(sizeMax);
                minPacketSize.Increment = new decimal(sizeStep);
                minPacketSize.Size = new System.Drawing.Size(70, 20);
                minPacketSize.Value = 8;
                minPacketSizes.Add(minPacketSize);
                

                // Максимальный размер пакета
                NumericUpDown maxPacketSize = new NumericUpDown();
                maxPacketSize.Location = new System.Drawing.Point(232, 46);
                maxPacketSize.Maximum = new decimal(sizeMax);
                maxPacketSize.Increment = new decimal(sizeStep);
                maxPacketSize.Size = new System.Drawing.Size(70, 20);
                maxPacketSize.Value = 128;
                maxPacketSizes.Add(maxPacketSize);
                

                // Минимальный промежуток времени между двумя пакетами в милисекундах
                NumericUpDown minTimePeriod = new NumericUpDown();
                minTimePeriod.Location = new System.Drawing.Point(232, 92);
                minTimePeriod.Maximum = new decimal(periodMax);
                minTimePeriod.Increment = new decimal(periodStep);
                minTimePeriod.Size = new System.Drawing.Size(70, 20);
                minTimePeriod.Value = 20;
                minTimePeriods.Add(minTimePeriod);

                // Максимальный промежуток времени между двумя пакетами в милисекундах
                NumericUpDown maxTimePeriod = new NumericUpDown();
                maxTimePeriod.Location = new System.Drawing.Point(232, 123);
                maxTimePeriod.Maximum = new decimal(periodMax);
                maxTimePeriod.Increment = new decimal(sizeStep);
                maxTimePeriod.Size = new System.Drawing.Size(70, 20);
                maxTimePeriod.Value = 100;
                maxTimePeriods.Add(maxTimePeriod);

                // Размер корзины
                NumericUpDown maxTokensCount = new NumericUpDown();
                maxTokensCount.Location = new System.Drawing.Point(232, 160);
                maxTokensCount.Maximum = new decimal(sizeMax);
                maxTokensCount.Increment = new decimal(periodStep);
                maxTokensCount.Size = new System.Drawing.Size(70, 20);
                maxTokensCount.Value = 128;
                maxTokensCounts.Add(maxTokensCount);

                // веса корзин
                TrackBar tokenBuketWeight = new TrackBar();
                tokenBuketWeight.Location = new System.Drawing.Point(218, 200);
                tokenBuketWeight.Size = new System.Drawing.Size(104, 45);
                tokenBuketWeight.BackColor = Color.White;
                tokenBuketWeight.Value = 10;
                tokenBuketWeights.Add(tokenBuketWeight);


                tabPage.AutoScroll = true;
                tabPage.Controls.Add(minPacketSize);
                tabPage.Controls.Add(maxPacketSize);
                tabPage.Controls.Add(minTimePeriod);
                tabPage.Controls.Add(maxTimePeriod);
                tabPage.Controls.Add(maxTokensCount);
                tabPage.Controls.Add(tokenBuketWeight);
                tabPage.Controls.Add(label1);
                tabPage.Controls.Add(label2);
                tabPage.Controls.Add(label3);
                tabPage.Controls.Add(label4);
                tabPage.Controls.Add(label5);
                tabPage.Controls.Add(label6);
                tabPage.Padding = new System.Windows.Forms.Padding(3);
                tabPage.TabIndex = i;
                tabPage.UseVisualStyleBackColor = true;

                // добавляем страницу
                this.generatorsSettings.Controls.Add(tabPage);
            }
        }

        private void bucketNum_ValueChanged(object sender, EventArgs e)
        {
            // меняем настройки генераторов
            InitGeneratorSettings();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.IsSettingsApplyed = false;
            this.Close();
        }

        private void apply_Click(object sender, EventArgs e)
        {
            this.IsSettingsApplyed = true;
            this.Close();
        }        
        
        private void averageStrategy_CheckedChanged(object sender, EventArgs e)
        {
            if((sender as RadioButton).Checked)
            {
                // инициализация QoS
                this.QoS = new AverageStrategyQos();
            }
        }

        private void simulatedAnnealing_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                // инициализация QoS
                this.QoS = new SimulatedAnnealingQoS();
            }
        }

        private void multiplexerSpeed_ValueChanged(object sender, EventArgs e)
        {
            multiplaxorSpeedDt.Value = multiplexerSpeed.Value * observationPeriod.Value;
        }

        private void multiplaxorSpeedDt_ValueChanged(object sender, EventArgs e)
        {
        }

        private void observationPeriod_ValueChanged(object sender, EventArgs e)
        {
            multiplaxorSpeedDt.Value = multiplexerSpeed.Value * observationPeriod.Value;
        }

        private void hillClimbing_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                // инициализация QoS
                this.QoS = new HillClimbingQos();
            }
        }

        private void harmonySearch_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                // инициализация QoS
                this.QoS = new HarmonySearchQos();
            }
        }       
    }
}
