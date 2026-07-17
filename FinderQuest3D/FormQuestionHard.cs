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
    public partial class FormQuestionHard : Form
    {
        FormRender renderForm;
        int selectedSlot = 0;

        public FormQuestionHard()
        {
            InitializeComponent();
        }
        private bool CheckAll()
        {
            return renderForm.activePersons.IsFinish();
        }
        private void UpdateQuestionText()
        {
            if (renderForm != null && renderForm.activePersons != null)
            {
                labelQuestion.Text = renderForm.activePersons.PersonQuestion[selectedSlot].Question;
                textBoxAnswer.Clear();
                textBoxAnswer.Focus();
            }
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                List<Questions> questions = renderForm.activePersons.PersonQuestion;
                int totalQuestions = questions.Count;
                if (renderForm.activePersons.CheckAnswer(textBoxAnswer.Text, selectedSlot) == true)
                {
                    MessageBox.Show($"Your answer is correct ! " +
                        $"\nYou get {renderForm.activePersons.PersonQuestion[selectedSlot].Score} points");
                    renderForm.player.AddScore(renderForm.activePersons.PersonQuestion[selectedSlot].Score);
                    renderForm.labelPlayer.Text = renderForm.player.DisplayData();
                    renderForm.time.AddWithSecond(20);
                    renderForm.activePersons.PersonQuestion[selectedSlot].Status = "V";
                }
                else
                {
                    MessageBox.Show("Your answer is incorrect ! ");
                    renderForm.activePersons.PersonQuestion[selectedSlot].Status = "X";
                }
                panelQuestion1.Invalidate();
                panelQuestion2.Invalidate();
                panelQuestion3.Invalidate();
                panelQuestion4.Invalidate();
                panelQuestion5.Invalidate();
                panelQuestion1.Update();
                panelQuestion2.Update();
                panelQuestion3.Update();
                panelQuestion4.Update();
                panelQuestion5.Update();
                int answeredCount = 0;
                foreach (var q in questions)
                {
                    if (q.Status == "V" || q.Status == "X")
                    {
                        answeredCount++;
                    }
                }

                // Only close the form if the player has attempted EVERY question in the list
                if (answeredCount >= totalQuestions)
                {
                    // Update the backend class properties so the main window knows you finished
                    renderForm.activePersons.SolvedStatus = true;
                    CheckAll();

                    renderForm.ExitTalkArea();
                    renderForm.PlaySound("walk");
                    this.Close();
                    return; // Exit out safely
                }
                bool foundNext = false;
                for (int i = 1; i <= totalQuestions; i++)
                {
                    int nextIndex = (selectedSlot + i) % totalQuestions;

                    // Skip questions that are already answered correctly
                    if (questions[nextIndex].Status != "V")
                    {
                        selectedSlot = nextIndex;
                        foundNext = true;
                        break;
                    }
                }

                // 5. Apply the UI update transitions
                if (foundNext)
                {
                    UpdateQuestionText();
                }
                else
                {
                    textBoxAnswer.Clear();
                    textBoxAnswer.Focus();
                }
            }
            catch (Exception ex)
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
        }

        private void FormQuestion1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.buttonSubmit_Click(sender, e);
            }
        }

        private void panelQuestion1_Click(object sender, EventArgs e)
        {

            if (renderForm.activePersons.PersonQuestion[0].Status != "V")
            {
                selectedSlot = 0;
                UpdateQuestionText();
            }
        }

        private void panelQuestion2_Click(object sender, EventArgs e)
        {
            if (renderForm.activePersons.PersonQuestion[1].Status != "V")
            {
                selectedSlot = 1;
                UpdateQuestionText();
            }
        }

        private void panelQuestion3_Click(object sender, EventArgs e)
        {
            if (renderForm.activePersons.PersonQuestion[2].Status != "V")
            {
                selectedSlot = 2;
                UpdateQuestionText();
            }
        }
        private void panelQuestion4_Click(object sender, EventArgs e)
        {
            if (renderForm.activePersons.PersonQuestion[3].Status != "V")
            {
                selectedSlot = 3;
                UpdateQuestionText();
            }
        }

        private void panelQuestion5_Click(object sender, EventArgs e)
        {
            if (renderForm.activePersons.PersonQuestion[4].Status != "V")
            {
                selectedSlot = 4;
                UpdateQuestionText();
            }
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

        private void panelQuestion4_Paint(object sender, PaintEventArgs e)
        {
            DrawStatusSymbol(panelQuestion4, 3, e);
        }

        private void panelQuestion5_Paint(object sender, PaintEventArgs e)
        {
            DrawStatusSymbol(panelQuestion5, 4, e);
        }
    }
}
