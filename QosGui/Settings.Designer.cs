namespace QosGui
{
    partial class Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.bucketNum = new System.Windows.Forms.NumericUpDown();
            this.apply = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.observationPeriod = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.generatorsSettings = new System.Windows.Forms.TabControl();
            this.multiplexerSpeed = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.seed = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.multiplexerWeight = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.queueWeight = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.queueLength = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.simulatedAnnealing = new System.Windows.Forms.RadioButton();
            this.peakStrategy = new System.Windows.Forms.RadioButton();
            this.averageStrategy = new System.Windows.Forms.RadioButton();
            this.historySize = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.multiplaxorSpeedDt = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bucketNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.observationPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.multiplexerSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.multiplexerWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.queueWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.queueLength)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.historySize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.multiplaxorSpeedDt)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Количество потоков";
            // 
            // bucketNum
            // 
            this.bucketNum.Location = new System.Drawing.Point(142, 11);
            this.bucketNum.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.bucketNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.bucketNum.Name = "bucketNum";
            this.bucketNum.Size = new System.Drawing.Size(47, 20);
            this.bucketNum.TabIndex = 1;
            this.bucketNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.bucketNum.ValueChanged += new System.EventHandler(this.bucketNum_ValueChanged);
            // 
            // apply
            // 
            this.apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.apply.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.apply.Location = new System.Drawing.Point(349, 454);
            this.apply.Name = "apply";
            this.apply.Size = new System.Drawing.Size(75, 23);
            this.apply.TabIndex = 3;
            this.apply.Text = "Применить";
            this.apply.UseVisualStyleBackColor = true;
            this.apply.Click += new System.EventHandler(this.apply_Click);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.Location = new System.Drawing.Point(430, 454);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 4;
            this.cancel.Text = "Отмена";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // observationPeriod
            // 
            this.observationPeriod.Location = new System.Drawing.Point(458, 60);
            this.observationPeriod.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.observationPeriod.Name = "observationPeriod";
            this.observationPeriod.Size = new System.Drawing.Size(44, 20);
            this.observationPeriod.TabIndex = 6;
            this.observationPeriod.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.observationPeriod.ValueChanged += new System.EventHandler(this.observationPeriod_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Cursor = System.Windows.Forms.Cursors.Default;
            this.label3.Location = new System.Drawing.Point(301, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Интервал наблюдения (мс)";
            // 
            // generatorsSettings
            // 
            this.generatorsSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.generatorsSettings.Location = new System.Drawing.Point(12, 175);
            this.generatorsSettings.Name = "generatorsSettings";
            this.generatorsSettings.SelectedIndex = 0;
            this.generatorsSettings.Size = new System.Drawing.Size(493, 272);
            this.generatorsSettings.TabIndex = 2;
            // 
            // multiplexerSpeed
            // 
            this.multiplexerSpeed.Location = new System.Drawing.Point(458, 35);
            this.multiplexerSpeed.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.multiplexerSpeed.Name = "multiplexerSpeed";
            this.multiplexerSpeed.Size = new System.Drawing.Size(44, 20);
            this.multiplexerSpeed.TabIndex = 7;
            this.multiplexerSpeed.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.multiplexerSpeed.ValueChanged += new System.EventHandler(this.multiplexerSpeed_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(231, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(214, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Ширна канала мультиплексора, байт/мс";
            // 
            // seed
            // 
            this.seed.Location = new System.Drawing.Point(142, 66);
            this.seed.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.seed.Name = "seed";
            this.seed.Size = new System.Drawing.Size(47, 20);
            this.seed.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(104, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Seed";
            // 
            // multiplexerWeight
            // 
            this.multiplexerWeight.Location = new System.Drawing.Point(360, 109);
            this.multiplexerWeight.Name = "multiplexerWeight";
            this.multiplexerWeight.Size = new System.Drawing.Size(145, 45);
            this.multiplexerWeight.TabIndex = 11;
            this.multiplexerWeight.Value = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Cursor = System.Windows.Forms.Cursors.Default;
            this.label5.Location = new System.Drawing.Point(198, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(166, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Вес потерь на мультиплексоре";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Cursor = System.Windows.Forms.Cursors.Default;
            this.label6.Location = new System.Drawing.Point(206, 141);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(157, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Вес очереди мультиплексора";
            // 
            // queueWeight
            // 
            this.queueWeight.Location = new System.Drawing.Point(359, 141);
            this.queueWeight.Name = "queueWeight";
            this.queueWeight.Size = new System.Drawing.Size(146, 45);
            this.queueWeight.TabIndex = 13;
            this.queueWeight.Value = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Cursor = System.Windows.Forms.Cursors.Default;
            this.label7.Location = new System.Drawing.Point(323, 91);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(122, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Длинна очереди (байт)";
            // 
            // queueLength
            // 
            this.queueLength.Location = new System.Drawing.Point(458, 89);
            this.queueLength.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.queueLength.Name = "queueLength";
            this.queueLength.Size = new System.Drawing.Size(44, 20);
            this.queueLength.TabIndex = 17;
            this.queueLength.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.simulatedAnnealing);
            this.panel1.Controls.Add(this.peakStrategy);
            this.panel1.Controls.Add(this.averageStrategy);
            this.panel1.Location = new System.Drawing.Point(5, 90);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(195, 79);
            this.panel1.TabIndex = 18;
            // 
            // simulatedAnnealing
            // 
            this.simulatedAnnealing.AutoSize = true;
            this.simulatedAnnealing.Location = new System.Drawing.Point(7, 47);
            this.simulatedAnnealing.Name = "simulatedAnnealing";
            this.simulatedAnnealing.Size = new System.Drawing.Size(115, 17);
            this.simulatedAnnealing.TabIndex = 2;
            this.simulatedAnnealing.Text = "Имитация отжига";
            this.simulatedAnnealing.UseVisualStyleBackColor = true;
            this.simulatedAnnealing.CheckedChanged += new System.EventHandler(this.simulatedAnnealing_CheckedChanged);
            // 
            // peakStrategy
            // 
            this.peakStrategy.AutoSize = true;
            this.peakStrategy.Location = new System.Drawing.Point(7, 24);
            this.peakStrategy.Name = "peakStrategy";
            this.peakStrategy.Size = new System.Drawing.Size(177, 17);
            this.peakStrategy.TabIndex = 1;
            this.peakStrategy.Text = "Стратегия пикового значения";
            this.peakStrategy.UseVisualStyleBackColor = true;
            this.peakStrategy.CheckedChanged += new System.EventHandler(this.peakStrategy_CheckedChanged);
            // 
            // averageStrategy
            // 
            this.averageStrategy.AutoSize = true;
            this.averageStrategy.Checked = true;
            this.averageStrategy.Location = new System.Drawing.Point(7, 1);
            this.averageStrategy.Name = "averageStrategy";
            this.averageStrategy.Size = new System.Drawing.Size(177, 17);
            this.averageStrategy.TabIndex = 0;
            this.averageStrategy.TabStop = true;
            this.averageStrategy.Text = "Стратегия среднего значения";
            this.averageStrategy.UseVisualStyleBackColor = true;
            this.averageStrategy.CheckedChanged += new System.EventHandler(this.averageStrategy_CheckedChanged);
            // 
            // historySize
            // 
            this.historySize.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.historySize.Location = new System.Drawing.Point(142, 37);
            this.historySize.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.historySize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.historySize.Name = "historySize";
            this.historySize.Size = new System.Drawing.Size(47, 20);
            this.historySize.TabIndex = 22;
            this.historySize.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 37);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(121, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Интервал наблюдения";
            // 
            // multiplaxorSpeedDt
            // 
            this.multiplaxorSpeedDt.Enabled = false;
            this.multiplaxorSpeedDt.InterceptArrowKeys = false;
            this.multiplaxorSpeedDt.Location = new System.Drawing.Point(458, 11);
            this.multiplaxorSpeedDt.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.multiplaxorSpeedDt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.multiplaxorSpeedDt.Name = "multiplaxorSpeedDt";
            this.multiplaxorSpeedDt.ReadOnly = true;
            this.multiplaxorSpeedDt.Size = new System.Drawing.Size(44, 20);
            this.multiplaxorSpeedDt.TabIndex = 23;
            this.multiplaxorSpeedDt.Value = new decimal(new int[] {
            1600,
            0,
            0,
            0});
            this.multiplaxorSpeedDt.ValueChanged += new System.EventHandler(this.multiplaxorSpeedDt_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(198, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(247, 13);
            this.label8.TabIndex = 24;
            this.label8.Text = "Ширна канала мультиплексора, байт/интервал";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(517, 489);
            this.ControlBox = false;
            this.Controls.Add(this.label8);
            this.Controls.Add(this.multiplaxorSpeedDt);
            this.Controls.Add(this.historySize);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.queueLength);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.generatorsSettings);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.queueWeight);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.multiplexerWeight);
            this.Controls.Add(this.seed);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.multiplexerSpeed);
            this.Controls.Add(this.observationPeriod);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.apply);
            this.Controls.Add(this.bucketNum);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки";
            ((System.ComponentModel.ISupportInitialize)(this.bucketNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.observationPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.multiplexerSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.multiplexerWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.queueWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.queueLength)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.historySize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.multiplaxorSpeedDt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button apply;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabControl generatorsSettings;
        private System.Windows.Forms.NumericUpDown bucketNum;
        private System.Windows.Forms.NumericUpDown observationPeriod;
        private System.Windows.Forms.NumericUpDown multiplexerSpeed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown seed;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar multiplexerWeight;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TrackBar queueWeight;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown queueLength;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton simulatedAnnealing;
        private System.Windows.Forms.RadioButton peakStrategy;
        private System.Windows.Forms.RadioButton averageStrategy;
        private System.Windows.Forms.NumericUpDown historySize;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown multiplaxorSpeedDt;
        private System.Windows.Forms.Label label8;
    }
}