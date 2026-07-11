using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            if (menu != null)
            {
                if (menu.activePersons.CheckAnswer(textBoxAnswer.Text) == true)
                {
                    MessageBox.Show($"Your answer is correct ! " +
                        $"\nYou get {menu.activePersons.PersonQuestion.Score} points");
                    menu.player.AddScore(menu.activePersons.PersonQuestion.Score);
                    menu.labelPlayer.Text = menu.player.DisplayData();
                }
                else MessageBox.Show("Your answer is incorrect ! ");
                menu.ExitTalkArea();
                menu.PlaySound("walk");
            }
            else if (renderForm != null)
            {
                if (renderForm.activePersons.CheckAnswer(textBoxAnswer.Text) == true)
                {
                    MessageBox.Show($"Your answer is correct ! " +
                        $"\nYou get {renderForm.activePersons.PersonQuestion.Score} points");
                    renderForm.player.AddScore(renderForm.activePersons.PersonQuestion.Score);
                    renderForm.labelPlayer.Text = renderForm.player.DisplayData();
                }
                else MessageBox.Show("Your answer is incorrect ! ");
                renderForm.ExitTalkArea();
                renderForm.PlaySound("walk");
            }
            this.Close();
        }
        
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void FormQuestion1_Load(object sender, EventArgs e)
        {
            if (this.Owner is FormMenu)
            {
                menu = (FormMenu)this.Owner;
                labelQuestion.Text = menu.activePersons.PersonQuestion.Question;
            }
            else if (this.Owner is FormRender)
            {
                renderForm = (FormRender)this.Owner;
                labelQuestion.Text = renderForm.activePersons.PersonQuestion.Question;
            }
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
