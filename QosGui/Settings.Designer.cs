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
            ((System.ComponentModel.ISupportInitialize)(this.bucketNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.observationPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.multiplexerSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Количество потоков";
            // 
            // bucketNum
            // 
            this.bucketNum.Location = new System.Drawing.Point(175, 45);
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
            this.bucketNum.Size = new System.Drawing.Size(44, 20);
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
            this.apply.Location = new System.Drawing.Point(316, 349);
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
            this.cancel.Location = new System.Drawing.Point(397, 349);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 4;
            this.cancel.Text = "Отмена";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // observationPeriod
            // 
            this.observationPeriod.Location = new System.Drawing.Point(422, 45);
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
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Cursor = System.Windows.Forms.Cursors.Default;
            this.label3.Location = new System.Drawing.Point(265, 47);
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
            this.generatorsSettings.Location = new System.Drawing.Point(12, 71);
            this.generatorsSettings.Name = "generatorsSettings";
            this.generatorsSettings.SelectedIndex = 0;
            this.generatorsSettings.Size = new System.Drawing.Size(460, 206);
            this.generatorsSettings.TabIndex = 2;
            // 
            // multiplexerSpeed
            // 
            this.multiplexerSpeed.Location = new System.Drawing.Point(422, 12);
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
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(195, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(214, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Ширна канала мультиплексора, байт/мс";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(484, 384);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.multiplexerSpeed);
            this.Controls.Add(this.observationPeriod);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.apply);
            this.Controls.Add(this.generatorsSettings);
            this.Controls.Add(this.bucketNum);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "Settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки";
            ((System.ComponentModel.ISupportInitialize)(this.bucketNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.observationPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.multiplexerSpeed)).EndInit();
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
    }
}