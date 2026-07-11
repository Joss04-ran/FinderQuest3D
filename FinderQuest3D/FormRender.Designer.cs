namespace FinderQuest3D
{
    partial class FormRender
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
            this.components = new System.ComponentModel.Container();
            this.panelDashboard = new System.Windows.Forms.Panel();
            this.labelTime = new System.Windows.Forms.Label();
            this.timerTime = new System.Windows.Forms.Timer(this.components);
            this.buttonExit = new System.Windows.Forms.Button();
            this.panelGameTop = new System.Windows.Forms.Panel();
            this.labelPlayer = new System.Windows.Forms.Label();
            this.labelArea = new System.Windows.Forms.Label();
            this.panelGameBottom = new System.Windows.Forms.Panel();
            this.labelTalkArea = new System.Windows.Forms.Label();
            this.panelDashboard.SuspendLayout();
            this.panelGameTop.SuspendLayout();
            this.panelGameBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelDashboard
            // 
            this.panelDashboard.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelDashboard.BackColor = System.Drawing.Color.DarkGray;
            this.panelDashboard.Controls.Add(this.labelTime);
            this.panelDashboard.Location = new System.Drawing.Point(13, 490);
            this.panelDashboard.Name = "panelDashboard";
            this.panelDashboard.Size = new System.Drawing.Size(1237, 171);
            this.panelDashboard.TabIndex = 0;
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Font = new System.Drawing.Font("Old English Text MT", 36F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTime.ForeColor = System.Drawing.Color.Black;
            this.labelTime.Location = new System.Drawing.Point(488, 9);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(258, 71);
            this.labelTime.TabIndex = 2;
            this.labelTime.Text = "00:00:00";
            // 
            // timerTime
            // 
            this.timerTime.Tick += new System.EventHandler(this.timerTime_Tick);
            // 
            // buttonExit
            // 
            this.buttonExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExit.Location = new System.Drawing.Point(1038, 25);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(211, 40);
            this.buttonExit.TabIndex = 1;
            this.buttonExit.Text = "EXIT GAME";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // panelGameTop
            // 
            this.panelGameTop.BackColor = System.Drawing.Color.Transparent;
            this.panelGameTop.Controls.Add(this.labelPlayer);
            this.panelGameTop.Controls.Add(this.labelArea);
            this.panelGameTop.Location = new System.Drawing.Point(12, 12);
            this.panelGameTop.Name = "panelGameTop";
            this.panelGameTop.Size = new System.Drawing.Size(1237, 108);
            this.panelGameTop.TabIndex = 5;
            // 
            // labelPlayer
            // 
            this.labelPlayer.AutoSize = true;
            this.labelPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer.Location = new System.Drawing.Point(1112, 13);
            this.labelPlayer.Name = "labelPlayer";
            this.labelPlayer.Size = new System.Drawing.Size(110, 22);
            this.labelPlayer.TabIndex = 2;
            this.labelPlayer.Text = "labelPlayer";
            // 
            // labelArea
            // 
            this.labelArea.AutoSize = true;
            this.labelArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelArea.Location = new System.Drawing.Point(18, 13);
            this.labelArea.Name = "labelArea";
            this.labelArea.Size = new System.Drawing.Size(95, 22);
            this.labelArea.TabIndex = 0;
            this.labelArea.Text = "labelArea";
            // 
            // panelGameBottom
            // 
            this.panelGameBottom.BackColor = System.Drawing.Color.Transparent;
            this.panelGameBottom.Controls.Add(this.labelTalkArea);
            this.panelGameBottom.Location = new System.Drawing.Point(4, -107);
            this.panelGameBottom.Name = "panelGameBottom";
            this.panelGameBottom.Size = new System.Drawing.Size(1255, 886);
            this.panelGameBottom.TabIndex = 6;
            // 
            // labelTalkArea
            // 
            this.labelTalkArea.AutoSize = true;
            this.labelTalkArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTalkArea.Location = new System.Drawing.Point(13, 11);
            this.labelTalkArea.Name = "labelTalkArea";
            this.labelTalkArea.Size = new System.Drawing.Size(134, 22);
            this.labelTalkArea.TabIndex = 3;
            this.labelTalkArea.Text = "labelTalkArea";
            // 
            // FormRender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 673);
            this.Controls.Add(this.panelGameBottom);
            this.Controls.Add(this.panelGameTop);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.panelDashboard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormRender";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FormRender_Load);
            this.panelDashboard.ResumeLayout(false);
            this.panelDashboard.PerformLayout();
            this.panelGameTop.ResumeLayout(false);
            this.panelGameTop.PerformLayout();
            this.panelGameBottom.ResumeLayout(false);
            this.panelGameBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelDashboard;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Timer timerTime;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Panel panelGameTop;
        public System.Windows.Forms.Label labelPlayer;
        private System.Windows.Forms.Label labelArea;
        public System.Windows.Forms.Panel panelGameBottom;
        private System.Windows.Forms.Label labelTalkArea;
    }
}