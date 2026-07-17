namespace FinderQuest3D
{
    partial class FormHighScore
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
            this.listBoxDisplay = new System.Windows.Forms.ListBox();
            this.comboBoxDisplay = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // listBoxDisplay
            // 
            this.listBoxDisplay.FormattingEnabled = true;
            this.listBoxDisplay.ItemHeight = 16;
            this.listBoxDisplay.Location = new System.Drawing.Point(64, 67);
            this.listBoxDisplay.Name = "listBoxDisplay";
            this.listBoxDisplay.Size = new System.Drawing.Size(660, 356);
            this.listBoxDisplay.TabIndex = 0;
            // 
            // comboBoxDisplay
            // 
            this.comboBoxDisplay.FormattingEnabled = true;
            this.comboBoxDisplay.Items.AddRange(new object[] {
            "All",
            "Win",
            "Lose"});
            this.comboBoxDisplay.Location = new System.Drawing.Point(64, 27);
            this.comboBoxDisplay.Name = "comboBoxDisplay";
            this.comboBoxDisplay.Size = new System.Drawing.Size(121, 24);
            this.comboBoxDisplay.TabIndex = 1;
            this.comboBoxDisplay.SelectedIndexChanged += new System.EventHandler(this.comboBoxDisplay_SelectedIndexChanged);
            // 
            // FormHighScore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::FinderQuest3D.Properties.Resources.backgroundQuestion;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.comboBoxDisplay);
            this.Controls.Add(this.listBoxDisplay);
            this.DoubleBuffered = true;
            this.Name = "FormHighScore";
            this.Text = "FormHighScore";
            this.Load += new System.EventHandler(this.FormHighScore_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxDisplay;
        private System.Windows.Forms.ComboBox comboBoxDisplay;
    }
}