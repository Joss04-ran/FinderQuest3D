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
                    panelAnswer.Visible = true;
                    labelAnswer.Visible = true;
                    labelAnswer.Text = $"Your answer is correct ! " +
                        $"\nYou get {renderForm.activePersons.PersonQuestion[selectedSlot].Score} points" +
                        $"\nAnd additional 10 second";
                    renderForm.player.AddScore(renderForm.activePersons.PersonQuestion[selectedSlot].Score);
                    renderForm.labelPlayer.Text = renderForm.player.DisplayData();
                    renderForm.time.AddWithSecond(10);
                    renderForm.activePersons.PersonQuestion[selectedSlot].Status = "V";
                }
                else
                {
                    panelAnswer.Visible = true;
                    labelAnswer.Visible = true;
                    labelAnswer.Text = "Your answer is incorrect !" +
                        "\nTimer is reduced by 5 second";
                    renderForm.activePersons.PersonQuestion[selectedSlot].Status = "X";
                    renderForm.time.AddWithSecond(-5);
                }
                panelQuestion1.Invalidate();
                panelQuestion2.Invalidate();
                panelQuestion3.Invalidate();
                panelQuestion1.Update();
                panelQuestion2.Update();
                panelQuestion3.Update();
                int answeredCount = 0;
                int correctAnswer = 0;
                foreach (var q in questions)
                {
                    if (q.Status == "V" || q.Status == "X")
                    {
                        answeredCount++;
                        if (q.Status == "V")
                            correctAnswer++;
                    }
                }
                if (answeredCount >= totalQuestions)
                {
                    renderForm.activePersons.SolvedStatus = true;
                    CheckAll();
                    MessageBox.Show($"You have answered {correctAnswer.ToString()} question correctly!");
                    renderForm.ExitTalkArea();
                    this.Close();
                    return;
                }
                bool foundNext = false;
                for (int i = 1; i <= totalQuestions; i++)
                {
                    int nextIndex = (selectedSlot + i) % totalQuestions;
                    if (questions[nextIndex].Status != "V")
                    {
                        selectedSlot = nextIndex;
                        foundNext = true;
                        break;
                    }
                }
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
            panelAnswer.Visible = false;
            labelAnswer.Visible = false;
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
                using (Pen greenPen = new Pen(Color.LimeGreen, 4))
                {
                    g.DrawLine(greenPen, panel.Width / 3, panel.Height / 2, panel.Width * 4 / 9, panel.Height * 2 / 3);
                    g.DrawLine(greenPen, panel.Width * 4 / 9, panel.Height * 2 / 3, panel.Width * 2 / 3, panel.Height / 3);
                }
            }
            else if (status == "X")
            {
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
    }
}
