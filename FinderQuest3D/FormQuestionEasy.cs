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
            catch { }
            this.Close();
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
                FormQuestion1_Load(sender, e);
            }
        }

        private void panelQuestion2_Click(object sender, EventArgs e)
        {
            if (renderForm.activePersons.PersonQuestion[1].Status != "V")
            {
                selectedSlot = 1;
                FormQuestion1_Load(sender, e);
            }
        }

        private void panelQuestion3_Click(object sender, EventArgs e)
        {
            if (renderForm.activePersons.PersonQuestion[2].Status != "V")
            {
                selectedSlot = 2;
                FormQuestion1_Load(sender, e);
            }
        }
    }
}
