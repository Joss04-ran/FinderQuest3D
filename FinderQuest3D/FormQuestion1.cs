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
    public partial class FormQuestion1 : Form
    {
        FormMenu menu;
        FormRender renderForm;

        public FormQuestion1()
        {
            InitializeComponent();
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (renderForm.activePersons.CheckAnswer(textBoxAnswer.Text) == true)
                {
                    MessageBox.Show($"Your answer is correct ! " +
                        $"\nYou get {renderForm.activePersons.PersonQuestion.Score} points");
                    renderForm.player.AddScore(renderForm.activePersons.PersonQuestion.Score);
                    renderForm.labelPlayer.Text = renderForm.player.DisplayData();
                    renderForm.time.AddWithSecond(10);
                }
                else MessageBox.Show("Your answer is incorrect ! ");
                renderForm.ExitTalkArea();
                renderForm.PlaySound("walk");
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
                labelQuestion.Text = renderForm.activePersons.PersonQuestion.Question;
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
    }
}
