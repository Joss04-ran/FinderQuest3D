using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace FinderQuest3D
{
    public partial class FormQuestionEasy : Form
    {
        FormRender renderForm;
        int selectedSlot = 0;

        public FormQuestionEasy()
        {
            InitializeComponent();
        }
        private bool CheckAll()
        {
           return renderForm.activePersons.IsFinish();
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (renderForm.activePersons.CheckAnswer(textBoxAnswer.Text, selectedSlot) == true)
                {
                    MessageBox.Show($"Your answer is correct ! " +
                        $"\nYou get {renderForm.activePersons.PersonQuestion[selectedSlot].Score} points");
                    renderForm.player.AddScore(renderForm.activePersons.PersonQuestion[selectedSlot].Score);
                    renderForm.labelPlayer.Text = renderForm.player.DisplayData();
                    renderForm.time.AddWithSecond(20);
                    renderForm.activePersons.PersonQuestion[selectedSlot].Status = "V";
                    CheckAll();
                }
                else
                {
                    MessageBox.Show("Your answer is incorrect ! ");
                    renderForm.activePersons.PersonQuestion[selectedSlot].Status = "X";
                }
                if (renderForm.activePersons.SolvedStatus == true)
                {
                    renderForm.ExitTalkArea();
                    renderForm.PlaySound("walk");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void FormQuestion1_Load(object sender, EventArgs e)
        {
            if (this.Owner is FormRender)
            {
                renderForm = (FormRender)this.Owner;
                labelQuestion.Text = renderForm.activePersons.PersonQuestion[selectedSlot].Question;
            }
            panelPersonProfile.BackgroundImage = null;
            panelPersonProfile.BackgroundImageLayout = ImageLayout.Stretch;
            renderForm.activePersons.DisplayPicture(panelPersonProfile);

            renderForm.activePersons.Picture.Size = panelPersonProfile.Size;
            renderForm.activePersons.Picture.Location = new Point(0, 0);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            this.Close();
        }

        private void panelQuestion1_Paint(object sender, PaintEventArgs e)
        {
            DrawStatusSymbol(panelQuestion1, 0, e);
        }

        private void panelQuestion2_Paint(object sender, PaintEventArgs e)
        {
            DrawStatusSymbol(panelQuestion2, 1, e);
        }

        private void panelQuestion3_Paint(object sender, PaintEventArgs e)
        {
            DrawStatusSymbol(panelQuestion3, 2, e);
        }
        private void DrawStatusSymbol(Panel panel, int slotIndex, PaintEventArgs e)
        {
            if (renderForm == null || renderForm.activePersons == null) return;
            List<Questions> questionsList = renderForm.activePersons.PersonQuestion;
            if (slotIndex >= questionsList.Count) return;

            string status = questionsList[slotIndex].Status;
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (status == "V")
            {
                // Draws a Green Checkmark inside the panel boundary lines
                using (Pen greenPen = new Pen(Color.LimeGreen, 4))
                {
                    g.DrawLine(greenPen, panel.Width / 3, panel.Height / 2, panel.Width * 4 / 9, panel.Height * 2 / 3);
                    g.DrawLine(greenPen, panel.Width * 4 / 9, panel.Height * 2 / 3, panel.Width * 2 / 3, panel.Height / 3);
                }
            }
            else if (status == "X")
            {
                // Draws a Red Cross (X) inside the panel boundary lines
                using (Pen redPen = new Pen(Color.Red, 4))
                {
                    int margin = Math.Min(panel.Width, panel.Height) / 3;
                    g.DrawLine(redPen, margin, margin, panel.Width - margin, panel.Height - margin);
                    g.DrawLine(redPen, panel.Width - margin, margin, margin, panel.Height - margin);
                }
            }
        }
    }
}
