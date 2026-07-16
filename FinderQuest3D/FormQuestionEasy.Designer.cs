namespace FinderQuest3D
{
    partial class FormQuestionEasy
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
            this.labelQuestion = new System.Windows.Forms.Label();
            this.buttonSubmit = new System.Windows.Forms.Button();
            this.textBoxAnswer = new System.Windows.Forms.TextBox();
            this.panelQuestion1 = new System.Windows.Forms.Panel();
            this.panelQuestion2 = new System.Windows.Forms.Panel();
            this.panelQuestion3 = new System.Windows.Forms.Panel();
            this.panelPersonProfile = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // labelQuestion
            // 
            this.labelQuestion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(193)))));
            this.labelQuestion.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelQuestion.Location = new System.Drawing.Point(355, 37);
            this.labelQuestion.Name = "labelQuestion";
            this.labelQuestion.Size = new System.Drawing.Size(414, 195);
            this.labelQuestion.TabIndex = 7;
            this.labelQuestion.Text = "\r\nSolve this Math Equation:\r\nx + y = 10\r\nIf x = 3, then y = ?\r\n";
            this.labelQuestion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelQuestion.Click += new System.EventHandler(this.label2_Click);
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(135)))));
            this.buttonSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSubmit.ForeColor = System.Drawing.Color.White;
            this.buttonSubmit.Location = new System.Drawing.Point(502, 305);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new System.Drawing.Size(119, 33);
            this.buttonSubmit.TabIndex = 6;
            this.buttonSubmit.Text = "Submit";
            this.buttonSubmit.UseVisualStyleBackColor = false;
            this.buttonSubmit.Click += new System.EventHandler(this.buttonSubmit_Click);
            // 
            // textBoxAnswer
            // 
            this.textBoxAnswer.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxAnswer.Location = new System.Drawing.Point(407, 250);
            this.textBoxAnswer.Name = "textBoxAnswer";
            this.textBoxAnswer.Size = new System.Drawing.Size(338, 36);
            this.textBoxAnswer.TabIndex = 5;
            // 
            // panelQuestion1
            // 
            this.panelQuestion1.BackColor = System.Drawing.SystemColors.Control;
            this.panelQuestion1.Location = new System.Drawing.Point(38, 37);
            this.panelQuestion1.Name = "panelQuestion1";
            this.panelQuestion1.Size = new System.Drawing.Size(78, 54);
            this.panelQuestion1.TabIndex = 8;
            this.panelQuestion1.Click += new System.EventHandler(this.panelQuestion1_Click);
            this.panelQuestion1.Paint += new System.Windows.Forms.PaintEventHandler(this.panelQuestion1_Paint);
            // 
            // panelQuestion2
            // 
            this.panelQuestion2.BackColor = System.Drawing.SystemColors.Control;
            this.panelQuestion2.Location = new System.Drawing.Point(38, 119);
            this.panelQuestion2.Name = "panelQuestion2";
            this.panelQuestion2.Size = new System.Drawing.Size(78, 54);
            this.panelQuestion2.TabIndex = 9;
            this.panelQuestion2.Click += new System.EventHandler(this.panelQuestion2_Click);
            this.panelQuestion2.Paint += new System.Windows.Forms.PaintEventHandler(this.panelQuestion2_Paint);
            // 
            // panelQuestion3
            // 
            this.panelQuestion3.BackColor = System.Drawing.SystemColors.Control;
            this.panelQuestion3.Location = new System.Drawing.Point(38, 206);
            this.panelQuestion3.Name = "panelQuestion3";
            this.panelQuestion3.Size = new System.Drawing.Size(78, 54);
            this.panelQuestion3.TabIndex = 10;
            this.panelQuestion3.Click += new System.EventHandler(this.panelQuestion3_Click);
            this.panelQuestion3.Paint += new System.Windows.Forms.PaintEventHandler(this.panelQuestion3_Paint);
            // 
            // panelPersonProfile
            // 
            this.panelPersonProfile.BackColor = System.Drawing.Color.Transparent;
            this.panelPersonProfile.BackgroundImage = global::FinderQuest3D.Properties.Resources.person3;
            this.panelPersonProfile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelPersonProfile.Location = new System.Drawing.Point(155, 37);
            this.panelPersonProfile.Name = "panelPersonProfile";
            this.panelPersonProfile.Size = new System.Drawing.Size(177, 281);
            this.panelPersonProfile.TabIndex = 11;
            // 
            // FormQuestionEasy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::FinderQuest3D.Properties.Resources.backgroundQuestion;
            this.ClientSize = new System.Drawing.Size(841, 391);
            this.Controls.Add(this.panelPersonProfile);
            this.Controls.Add(this.panelQuestion3);
            this.Controls.Add(this.panelQuestion2);
            this.Controls.Add(this.panelQuestion1);
            this.Controls.Add(this.labelQuestion);
            this.Controls.Add(this.buttonSubmit);
            this.Controls.Add(this.textBoxAnswer);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormQuestionEasy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FormQuestion1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormQuestion1_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelQuestion;
        private System.Windows.Forms.Button buttonSubmit;
        private System.Windows.Forms.TextBox textBoxAnswer;
        private System.Windows.Forms.Panel panelQuestion1;
        private System.Windows.Forms.Panel panelQuestion2;
        private System.Windows.Forms.Panel panelQuestion3;
        private System.Windows.Forms.Panel panelPersonProfile;
    }
}