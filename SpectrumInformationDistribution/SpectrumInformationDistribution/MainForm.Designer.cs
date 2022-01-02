namespace SpectrumInformationDistribution
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_startbroadcast = new System.Windows.Forms.Button();
            this.button_broadband = new System.Windows.Forms.Button();
            this.labelState = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_startbroadcast
            // 
            this.button_startbroadcast.Location = new System.Drawing.Point(12, 12);
            this.button_startbroadcast.Name = "button_startbroadcast";
            this.button_startbroadcast.Size = new System.Drawing.Size(121, 27);
            this.button_startbroadcast.TabIndex = 0;
            this.button_startbroadcast.Text = "Start broadcasting";
            this.button_startbroadcast.UseVisualStyleBackColor = true;
            this.button_startbroadcast.Click += new System.EventHandler(this.button_startbroadcast_Click);
            // 
            // button_broadband
            // 
            this.button_broadband.Location = new System.Drawing.Point(139, 12);
            this.button_broadband.Name = "button_broadband";
            this.button_broadband.Size = new System.Drawing.Size(133, 27);
            this.button_broadband.TabIndex = 1;
            this.button_broadband.Text = "Stop broadcasting";
            this.button_broadband.UseVisualStyleBackColor = true;
            this.button_broadband.Click += new System.EventHandler(this.button_broadband_Click);
            // 
            // labelState
            // 
            this.labelState.AutoSize = true;
            this.labelState.Location = new System.Drawing.Point(67, 69);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(0, 12);
            this.labelState.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 57);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.button_broadband);
            this.Controls.Add(this.button_startbroadcast);
            this.Name = "MainForm";
            this.Text = "SystemAudioAction Server";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_startbroadcast;
        private System.Windows.Forms.Button button_broadband;
        private System.Windows.Forms.Label labelState;
    }
}

