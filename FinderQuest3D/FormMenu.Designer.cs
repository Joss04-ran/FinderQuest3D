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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMenu));
            this.buttonPlay = new System.Windows.Forms.Button();
            this.buttonTutorial = new System.Windows.Forms.Button();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonPlay
            // 
            this.buttonPlay.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonPlay.BackgroundImage")));
            this.buttonPlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonPlay.Font = new System.Drawing.Font("Palatino Linotype", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPlay.Location = new System.Drawing.Point(527, 404);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(252, 57);
            this.buttonPlay.TabIndex = 4;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonTutorial
            // 
            this.buttonTutorial.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonTutorial.BackgroundImage")));
            this.buttonTutorial.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonTutorial.Font = new System.Drawing.Font("Palatino Linotype", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTutorial.Location = new System.Drawing.Point(527, 480);
            this.buttonTutorial.Name = "buttonTutorial";
            this.buttonTutorial.Size = new System.Drawing.Size(252, 62);
            this.buttonTutorial.TabIndex = 5;
            this.buttonTutorial.Text = "How To Play";
            this.buttonTutorial.UseVisualStyleBackColor = true;
            this.buttonTutorial.Click += new System.EventHandler(this.buttonTutorial_Click);
            // 
            // buttonQuit
            // 
            this.buttonQuit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonQuit.BackgroundImage")));
            this.buttonQuit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonQuit.Font = new System.Drawing.Font("Palatino Linotype", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonQuit.Location = new System.Drawing.Point(527, 560);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(252, 57);
            this.buttonQuit.TabIndex = 6;
            this.buttonQuit.Text = "Quit";
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.buttonQuit_Click);
            // 
            // FormMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::FinderQuest3D.Properties.Resources.background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1262, 673);
            this.Controls.Add(this.buttonQuit);
            this.Controls.Add(this.buttonTutorial);
            this.Controls.Add(this.buttonPlay);
            this.DoubleBuffered = true;
            this.Name = "FormMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Finder Quest Game";
            this.Load += new System.EventHandler(this.FormMenu_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Button buttonTutorial;
        private System.Windows.Forms.Button buttonQuit;
    }
}