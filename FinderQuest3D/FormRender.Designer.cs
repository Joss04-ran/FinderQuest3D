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
            this.panelGameTop = new System.Windows.Forms.Panel();
            this.labelTime = new System.Windows.Forms.Label();
            this.panelMinimap = new System.Windows.Forms.Panel();
            this.panelPlayerProfile = new System.Windows.Forms.Panel();
            this.labelPlayer = new System.Windows.Forms.Label();
            this.labelArea = new System.Windows.Forms.Label();
            this.timerTime = new System.Windows.Forms.Timer(this.components);
            this.panelGameBottom = new System.Windows.Forms.Panel();
            this.labelTalkArea = new System.Windows.Forms.Label();
            this.buttonExit = new System.Windows.Forms.Button();
            this.menuStripSettings = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemPlayPause = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMuteUnmute = new System.Windows.Forms.ToolStripMenuItem();
            this.panelDashboard.SuspendLayout();
            this.panelGameTop.SuspendLayout();
            this.panelGameBottom.SuspendLayout();
            this.menuStripSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelDashboard
            // 
            this.panelDashboard.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelDashboard.BackColor = System.Drawing.Color.SeaShell;
            this.panelDashboard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDashboard.Controls.Add(this.panelGameTop);
            this.panelDashboard.ForeColor = System.Drawing.Color.Black;
            this.panelDashboard.Location = new System.Drawing.Point(12, 545);
            this.panelDashboard.Name = "panelDashboard";
            this.panelDashboard.Size = new System.Drawing.Size(1256, 171);
            this.panelDashboard.TabIndex = 0;
            // 
            // panelGameTop
            // 
            this.panelGameTop.BackColor = System.Drawing.Color.Transparent;
            this.panelGameTop.Controls.Add(this.labelTime);
            this.panelGameTop.Controls.Add(this.panelMinimap);
            this.panelGameTop.Controls.Add(this.panelPlayerProfile);
            this.panelGameTop.Controls.Add(this.labelPlayer);
            this.panelGameTop.Controls.Add(this.labelArea);
            this.panelGameTop.Location = new System.Drawing.Point(3, 5);
            this.panelGameTop.Name = "panelGameTop";
            this.panelGameTop.Size = new System.Drawing.Size(1248, 165);
            this.panelGameTop.TabIndex = 5;
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Font = new System.Drawing.Font("Old English Text MT", 36F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTime.ForeColor = System.Drawing.Color.Black;
            this.labelTime.Location = new System.Drawing.Point(412, 13);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(258, 71);
            this.labelTime.TabIndex = 2;
            this.labelTime.Text = "00:00:00";
            // 
            // panelMinimap
            // 
            this.panelMinimap.Location = new System.Drawing.Point(776, 13);
            this.panelMinimap.Name = "panelMinimap";
            this.panelMinimap.Size = new System.Drawing.Size(140, 140);
            this.panelMinimap.TabIndex = 3;
            // 
            // panelPlayerProfile
            // 
            this.panelPlayerProfile.Location = new System.Drawing.Point(220, 13);
            this.panelPlayerProfile.Name = "panelPlayerProfile";
            this.panelPlayerProfile.Size = new System.Drawing.Size(140, 140);
            this.panelPlayerProfile.TabIndex = 3;
            // 
            // labelPlayer
            // 
            this.labelPlayer.AutoSize = true;
            this.labelPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer.Location = new System.Drawing.Point(997, 13);
            this.labelPlayer.Name = "labelPlayer";
            this.labelPlayer.Size = new System.Drawing.Size(198, 22);
            this.labelPlayer.TabIndex = 2;
            this.labelPlayer.Text = "Name = Lorem Ipsum";
            this.labelPlayer.Click += new System.EventHandler(this.labelPlayer_Click);
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
            // timerTime
            // 
            this.timerTime.Tick += new System.EventHandler(this.timerTime_Tick);
            // 
            // panelGameBottom
            // 
            this.panelGameBottom.BackColor = System.Drawing.Color.Transparent;
            this.panelGameBottom.Controls.Add(this.labelTalkArea);
            this.panelGameBottom.Controls.Add(this.buttonExit);
            this.panelGameBottom.Location = new System.Drawing.Point(0, 1);
            this.panelGameBottom.Name = "panelGameBottom";
            this.panelGameBottom.Size = new System.Drawing.Size(1280, 715);
            this.panelGameBottom.TabIndex = 6;
            // 
            // labelTalkArea
            // 
            this.labelTalkArea.AutoSize = true;
            this.labelTalkArea.Font = new System.Drawing.Font("Palatino Linotype", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTalkArea.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.labelTalkArea.Location = new System.Drawing.Point(10, 34);
            this.labelTalkArea.Name = "labelTalkArea";
            this.labelTalkArea.Size = new System.Drawing.Size(163, 31);
            this.labelTalkArea.TabIndex = 3;
            this.labelTalkArea.Text = "labelTalkArea";
            // 
            // buttonExit
            // 
            this.buttonExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExit.Location = new System.Drawing.Point(1047, 16);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(211, 40);
            this.buttonExit.TabIndex = 1;
            this.buttonExit.Text = "EXIT GAME";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // menuStripSettings
            // 
            this.menuStripSettings.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStripSettings.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripSettings.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSettings});
            this.menuStripSettings.Location = new System.Drawing.Point(0, 0);
            this.menuStripSettings.Name = "menuStripSettings";
            this.menuStripSettings.Size = new System.Drawing.Size(1280, 35);
            this.menuStripSettings.TabIndex = 7;
            this.menuStripSettings.Text = "menuStrip1";
            // 
            // toolStripMenuItemSettings
            // 
            this.toolStripMenuItemSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemPlayPause,
            this.toolStripMenuItemMuteUnmute});
            this.toolStripMenuItemSettings.Name = "toolStripMenuItemSettings";
            this.toolStripMenuItemSettings.Size = new System.Drawing.Size(99, 31);
            this.toolStripMenuItemSettings.Text = "Settings";
            // 
            // toolStripMenuItemPlayPause
            // 
            this.toolStripMenuItemPlayPause.Name = "toolStripMenuItemPlayPause";
            this.toolStripMenuItemPlayPause.Size = new System.Drawing.Size(299, 32);
            this.toolStripMenuItemPlayPause.Text = "Play / Pause";
            this.toolStripMenuItemPlayPause.Click += new System.EventHandler(this.toolStripMenuItemPlayPause_Click);
            // 
            // toolStripMenuItemMuteUnmute
            // 
            this.toolStripMenuItemMuteUnmute.Name = "toolStripMenuItemMuteUnmute";
            this.toolStripMenuItemMuteUnmute.Size = new System.Drawing.Size(299, 32);
            this.toolStripMenuItemMuteUnmute.Text = "Mute / Unmute Music";
            this.toolStripMenuItemMuteUnmute.Click += new System.EventHandler(this.toolStripMenuItemMuteUnmute_Click);
            // 
            // FormRender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.menuStripSettings);
            this.Controls.Add(this.panelDashboard);
            this.Controls.Add(this.panelGameBottom);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormRender";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FormRender_Load);
            this.panelDashboard.ResumeLayout(false);
            this.panelGameTop.ResumeLayout(false);
            this.panelGameTop.PerformLayout();
            this.panelGameBottom.ResumeLayout(false);
            this.panelGameBottom.PerformLayout();
            this.menuStripSettings.ResumeLayout(false);
            this.menuStripSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelDashboard;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Timer timerTime;
        private System.Windows.Forms.Panel panelGameTop;
        public System.Windows.Forms.Label labelPlayer;
        private System.Windows.Forms.Label labelArea;
        public System.Windows.Forms.Panel panelGameBottom;
        private System.Windows.Forms.Label labelTalkArea;
        private System.Windows.Forms.Panel panelPlayerProfile;
        private System.Windows.Forms.Panel panelMinimap;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.MenuStrip menuStripSettings;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSettings;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPlayPause;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMuteUnmute;
    }
}