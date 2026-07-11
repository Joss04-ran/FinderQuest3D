namespace FinderQuest3D
{
    partial class FormMenu
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
            this.panelGameBottom = new System.Windows.Forms.Panel();
            this.labelTalkArea = new System.Windows.Forms.Label();
            this.labelArea = new System.Windows.Forms.Label();
            this.labelTime = new System.Windows.Forms.Label();
            this.labelPlayer = new System.Windows.Forms.Label();
            this.panelGameTop = new System.Windows.Forms.Panel();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemStartGame = new System.Windows.Forms.ToolStripMenuItem();
            this.playPauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.timerTime1 = new System.Windows.Forms.Timer(this.components);
            this.panelGameBottom.SuspendLayout();
            this.panelGameTop.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelGameBottom
            // 
            this.panelGameBottom.BackColor = System.Drawing.Color.Transparent;
            this.panelGameBottom.Controls.Add(this.labelTalkArea);
            this.panelGameBottom.Location = new System.Drawing.Point(0, 145);
            this.panelGameBottom.Name = "panelGameBottom";
            this.panelGameBottom.Size = new System.Drawing.Size(1255, 886);
            this.panelGameBottom.TabIndex = 5;
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
            // labelArea
            // 
            this.labelArea.AutoSize = true;
            this.labelArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelArea.Location = new System.Drawing.Point(13, 4);
            this.labelArea.Name = "labelArea";
            this.labelArea.Size = new System.Drawing.Size(95, 22);
            this.labelArea.TabIndex = 0;
            this.labelArea.Text = "labelArea";
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTime.ForeColor = System.Drawing.Color.Red;
            this.labelTime.Location = new System.Drawing.Point(498, 35);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(270, 69);
            this.labelTime.TabIndex = 1;
            this.labelTime.Text = "00:00:00";
            // 
            // labelPlayer
            // 
            this.labelPlayer.AutoSize = true;
            this.labelPlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer.Location = new System.Drawing.Point(1006, 4);
            this.labelPlayer.Name = "labelPlayer";
            this.labelPlayer.Size = new System.Drawing.Size(110, 22);
            this.labelPlayer.TabIndex = 2;
            this.labelPlayer.Text = "labelPlayer";
            // 
            // panelGameTop
            // 
            this.panelGameTop.BackColor = System.Drawing.Color.Transparent;
            this.panelGameTop.Controls.Add(this.labelPlayer);
            this.panelGameTop.Controls.Add(this.labelTime);
            this.panelGameTop.Controls.Add(this.labelArea);
            this.panelGameTop.Location = new System.Drawing.Point(0, 31);
            this.panelGameTop.Name = "panelGameTop";
            this.panelGameTop.Size = new System.Drawing.Size(1255, 108);
            this.panelGameTop.TabIndex = 4;
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemStartGame,
            this.playPauseToolStripMenuItem});
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(62, 24);
            this.gameToolStripMenuItem.Text = "Game";
            // 
            // toolStripMenuItemStartGame
            // 
            this.toolStripMenuItemStartGame.Name = "toolStripMenuItemStartGame";
            this.toolStripMenuItemStartGame.Size = new System.Drawing.Size(200, 26);
            this.toolStripMenuItemStartGame.Text = "Start New Game";
            this.toolStripMenuItemStartGame.Click += new System.EventHandler(this.toolStripMenuItemStartGame_Click);
            // 
            // playPauseToolStripMenuItem
            // 
            this.playPauseToolStripMenuItem.Name = "playPauseToolStripMenuItem";
            this.playPauseToolStripMenuItem.Size = new System.Drawing.Size(200, 26);
            this.playPauseToolStripMenuItem.Text = "Play / Pause";
            this.playPauseToolStripMenuItem.Click += new System.EventHandler(this.playPauseToolStripMenuItem_Click);
            // 
            // toolStripMenuItemHelp
            // 
            this.toolStripMenuItemHelp.Name = "toolStripMenuItemHelp";
            this.toolStripMenuItemHelp.Size = new System.Drawing.Size(55, 24);
            this.toolStripMenuItemHelp.Text = "Help";
            this.toolStripMenuItemHelp.Click += new System.EventHandler(this.toolStripMenuItemHelp_Click);
            // 
            // toolStripMenuItemExit
            // 
            this.toolStripMenuItemExit.Name = "toolStripMenuItemExit";
            this.toolStripMenuItemExit.Size = new System.Drawing.Size(47, 24);
            this.toolStripMenuItemExit.Text = "Exit";
            this.toolStripMenuItemExit.Click += new System.EventHandler(this.toolStripMenuItemExit_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameToolStripMenuItem,
            this.toolStripMenuItemHelp,
            this.toolStripMenuItemExit,
            this.toolStripMenuItemAbout});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1255, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItemAbout
            // 
            this.toolStripMenuItemAbout.Name = "toolStripMenuItemAbout";
            this.toolStripMenuItemAbout.Size = new System.Drawing.Size(64, 24);
            this.toolStripMenuItemAbout.Text = "About";
            this.toolStripMenuItemAbout.Click += new System.EventHandler(this.toolStripMenuItemAbout_Click);
            // 
            // timerTime1
            // 
            //this.timerTime1.Tick += new System.EventHandler(this.timerTime1_Tick);
            // 
            // FormMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::FinderQuest3D.Properties.Resources.background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1255, 677);
            this.Controls.Add(this.panelGameTop);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panelGameBottom);
            this.DoubleBuffered = true;
            this.Name = "FormMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Finder Quest Game";
            this.Load += new System.EventHandler(this.FormMenu_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMenu_KeyDown);
            this.panelGameBottom.ResumeLayout(false);
            this.panelGameBottom.PerformLayout();
            this.panelGameTop.ResumeLayout(false);
            this.panelGameTop.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelArea;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Panel panelGameTop;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemStartGame;
        private System.Windows.Forms.ToolStripMenuItem playPauseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExit;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Timer timerTime1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAbout;
        private System.Windows.Forms.Label labelTalkArea;
        public System.Windows.Forms.Panel panelGameBottom;
        public System.Windows.Forms.Label labelPlayer;
    }
}