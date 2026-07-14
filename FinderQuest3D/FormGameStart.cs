using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinderQuest3D
{
    public partial class FormGameStart : Form
    {
        public string difficult = "";
        public string name = "";
        FormMenu form;
        public FormGameStart()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            name = textBoxName.Text;
            this.Hide();
            try
            {
                if (difficult != "")
                {
                    using (FormRender form = new FormRender(this))
                    {
                        form.ShowDialog();
                    }
                }
                else throw new Exception("Pick the difficult first!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FormGameStart_Load(object sender, EventArgs e)
        {
            form = (FormMenu)this.Owner;
            name = textBoxName.Text;
        }

        private void buttonHard_Click(object sender, EventArgs e)
        {
            difficult = "hard";
            panelEasy.BackColor = Color.Transparent;
            panelHard.BackColor = Color.White;
        }

        private void buttonEasy_Click(object sender, EventArgs e)
        {
            difficult = "easy";
            panelEasy.BackColor = Color.White;
            panelHard.BackColor = Color.Transparent;
        }
    }
}
