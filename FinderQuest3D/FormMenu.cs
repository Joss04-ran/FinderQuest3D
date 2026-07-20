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
        public WindowsMediaPlayer soundPlayer;
        public Persons activePersons = null;

        public FormMenu()
        {
            InitializeComponent();
            soundPlayer = new WindowsMediaPlayer();
        }

        public void PlaySound(string type)
        {
            if (type == "menu")
                soundPlayer.URL = Application.StartupPath + "\\sound\\BacksoundWalkArea.mp3";
            else if (type == "main")
                soundPlayer.URL = Application.StartupPath + "\\sound\\LessonMode.mp3";
            else if (type == "lose")
                soundPlayer.URL = Application.StartupPath + "\\sound\\LoseGame.mp3";
            else if (type == "win")
                soundPlayer.URL = Application.StartupPath + "\\sound\\WinGame.mp3";
            soundPlayer.controls.play();
        }

        private void FormMenu_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            this.DoubleBuffered = true;
            PlaySound("menu");
        }

        private void StartGame()
        {
            FormGameStart form = new FormGameStart();
            form.Owner = this;
            form.ShowDialog();
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
            MessageBox.Show("Press WASD to move player" +
"\nPress Arrow left or right to rotate" +
"\nPress enter to talk with the person" +
"\nPress Y to answer the question" +
"\nPress Esc to exit talk area", "How to play", MessageBoxButtons.OK);
        }
    }
}
