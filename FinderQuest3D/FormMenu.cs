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
        bool isStart = false;
        bool isPause = false;
        public Players player;
        // No Polymorphism
        // Only 1 walk area and 1 talk area (also for efficiency)
        WalkAreas walkAreas = null;
        TalkAreas talkAreas = null;
        bool enterTalkAreas = false;
        WindowsMediaPlayer soundPlayer = new WindowsMediaPlayer();
        public Persons activePersons = null;
        Point lastLocation; // save previous location
        Size lastSize; // save previous size

        public FormMenu()
        {
            InitializeComponent();
        }
        private void FormMenu_Load(object sender, EventArgs e)
        {
            panelGameBottom.Visible = false;
            panelGameTop.Visible = false;
            playPauseToolStripMenuItem.Enabled = false;
            labelTalkArea.Visible = false;
            timerTime1.Interval = 1000;
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

        private void toolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by Ubaya Student at West Campus" +
    "\nJonathan - 160425085" +
    "\nEdbert - 160425095" +
    "\nMade in Windows Form", "About Us", MessageBoxButtons.OK);
        }

        private void toolStripMenuItemHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Press arrow key (or WASD) to move player" +
    "\nPress enter to talk with the person" +
    "\nPress Y to answer the question" +
    "\nPress Esc to exit talk area", "How to play", MessageBoxButtons.OK);
        }

        private void toolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripMenuItemStartGame_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void playPauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (isPause == false)
            //{
            //    PauseGame();
            //}
            //else
            //{
            //    ContinueGame();
            //}
        }
    }
}
