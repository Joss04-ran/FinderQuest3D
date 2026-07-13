using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib; // library for play .mp3 sound

namespace FinderQuest3D
{
    public partial class FormMenu : Form
    {
        public bool isStart = false;
        public bool isPause = false;
        public Players player;
        WindowsMediaPlayer soundPlayer = new WindowsMediaPlayer();
        public Persons activePersons = null;

        public FormMenu()
        {
            InitializeComponent();
        }
        private void FormMenu_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            this.DoubleBuffered = true;
        }

        private void StartGame()
        {
            using (FormRender form = new FormRender())
            {
                form.Owner = this;
                form.ShowDialog();
            }
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonTutorial_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Press arrow key (or WASD) to move player" +
"\nPress enter to talk with the person" +
"\nPress Y to answer the question" +
"\nPress Esc to exit talk area", "How to play", MessageBoxButtons.OK);
        }
    }
}
