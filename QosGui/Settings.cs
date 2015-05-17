﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QosGui
{
    public partial class Settings : Form
    {
        /// <summary>
        /// возращает истину, если были применены новые параметры
        /// </summary>
        public bool IsSettingsApplyed { get; private set; }

        /// <summary>
        /// список хранит минимальные размеры пакетов 
        /// </summary>
        private List<NumericUpDown> minPacketSizes = new List<NumericUpDown>();

        /// <summary>
        /// список хранит максимальные размеры пакетов
        /// </summary>
        private List<NumericUpDown> maxPacketSizes = new List<NumericUpDown>();

        /// <summary>
        /// список хранит минимальные промежутоки времени между двумя пакетами в милисекундах
        /// </summary>
        private List<NumericUpDown> minTimePeriods = new List<NumericUpDown>();

        /// <summary>
        /// список хранит максимальные промежутоки времени между двумя пакетами в милисекундах
        /// </summary>
        private List<NumericUpDown> maxTimePeriods = new List<NumericUpDown>();


        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            // инициализация окна настроек
            InitGeneratorSettings();
            this.IsSettingsApplyed = false;
        }

        /// <summary>
        /// Инициализация объекта generatorSettings
        /// </summary>
        private void InitGeneratorSettings()
        {
            // максимальный период
            int periodMax = 10000;

            // подготовка к инициализации
            this.generatorsSettings.TabPages.Clear();
            minPacketSizes.Clear();
            maxPacketSizes.Clear();
            minTimePeriods.Clear();
            maxTimePeriods.Clear();

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
                label1.Text = "Минимальный размер пакета";

                Label label2 = new Label();
                label2.AutoSize = true;
                label2.Location = new System.Drawing.Point(16, 48);
                label2.Size = new System.Drawing.Size(165, 13);
                label2.Text = "Максимальный размер пакета";

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

                // создаём нумерики
                // Минимальный размер пакета
                NumericUpDown minPacketSize = new NumericUpDown();
                minPacketSize.Location = new System.Drawing.Point(232, 16);
                minPacketSize.Maximum = new decimal(periodMax);
                minPacketSize.Size = new System.Drawing.Size(44, 20);
                minPacketSizes.Add(minPacketSize);

                // Максимальный размер пакета
                NumericUpDown maxPacketSize = new NumericUpDown();
                maxPacketSize.Location = new System.Drawing.Point(232, 46);
                minPacketSize.Maximum = new decimal(periodMax);
                maxPacketSize.Size = new System.Drawing.Size(44, 20);
                maxPacketSizes.Add(maxPacketSize);

                // Минимальный промежуток времени между двумя пакетами в милисекундах
                NumericUpDown minTimePeriod = new NumericUpDown();
                minTimePeriod.Location = new System.Drawing.Point(232, 92);
                minPacketSize.Maximum = new decimal(periodMax);
                minTimePeriod.Size = new System.Drawing.Size(44, 20);
                minTimePeriods.Add(minTimePeriod);

                // Минимальный промежуток времени между двумя пакетами в милисекундах
                NumericUpDown maxTimePeriod = new NumericUpDown();
                maxTimePeriod.Location = new System.Drawing.Point(232, 124);
                maxTimePeriod.Maximum = new decimal(periodMax);
                maxTimePeriod.Size = new System.Drawing.Size(44, 20);
                maxTimePeriods.Add(maxTimePeriod);



                tabPage.AutoScroll = true;
                tabPage.Controls.Add(minPacketSize);
                tabPage.Controls.Add(maxPacketSize);
                tabPage.Controls.Add(minTimePeriod);
                tabPage.Controls.Add(maxTimePeriod);
                tabPage.Controls.Add(label1);
                tabPage.Controls.Add(label2);
                tabPage.Controls.Add(label3);
                tabPage.Controls.Add(label4);
        //        tabPage.Location = new System.Drawing.Point(4, 22);
                tabPage.Padding = new System.Windows.Forms.Padding(3);
       //         tabPage.Size = new System.Drawing.Size(674, 194);
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


        // TODO: публичные методы, чтоды вытягивать инфу из списков minPacketSizes и тд
    }
}
