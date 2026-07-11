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
            //PlaySound("walk");
            //panelGameBottom.Visible = false;
            //panelGameTop.Visible = true;
            //time = new Time(0, 30, 0);
            //isStart = true;
            //timerTime1.Start();
            //toolStripMenuItemStartGame.Enabled = false;
            //playPauseToolStripMenuItem.Enabled = true;
            //player = new Players("Jonathan", FinderQuest3D.Properties.Resources.player_right, new Size(80, 110)
            //    , new Point (10, 420), time);
            //player.DisplayPicture(this);
            //labelPlayer.Text = player.DisplayData();
            //GenerateWalkArea();
            using (FormRender form = new FormRender())
            {
                form.Owner = this;
                form.ShowDialog();
            }
        }

        private void PauseGame()
        {
            timerTime1.Stop();
            labelTime.Text = "Paused";
            isPause = true;
        }

        private void ContinueGame()
        {
            timerTime1.Start();
            isPause = false;
        }

        public void PlaySound(string type)
        {
            if (type == "walk")
                soundPlayer.URL = Application.StartupPath + "\\sound\\BacksoundWalkArea.mp3";
            else if (type == "talk")
                soundPlayer.URL = Application.StartupPath + "\\sound\\BacksoundTalkArea.mp3";
            else if (type == "lose")
                soundPlayer.URL = Application.StartupPath + "\\sound\\LoseGame.mp3";
            else if (type == "win")
                soundPlayer.URL = Application.StartupPath + "\\sound\\WinGame.mp3";
            soundPlayer.controls.play();
        }

        private void PauseSound()
        {
            soundPlayer.controls.pause();
        }

        public void GameOver()
        {
            PauseSound();
            timerTime1.Stop();
            isStart = false;
            if (player != null) player.HidePicture(this);
            toolStripMenuItemStartGame.Enabled = true;
            playPauseToolStripMenuItem.Enabled = false;
            panelGameBottom.Visible = false;
            panelGameTop.Visible = false;
            walkAreas.RemovePerson();
            this.BackgroundImage = FinderQuest3D.Properties.Resources.background;
        }

        private void GenerateTalkArea()
        {
            PlaySound("talk");
            enterTalkAreas = true;
            if (activePersons.NoPerson == 1.ToString())
            {
                talkAreas = new TalkAreas("Anna's House", FinderQuest3D.Properties.Resources.talkArea1, activePersons);
                activePersons.AddQuestion("Solve this math equation: " +
                    "\r\n x + y = 10 " +
                    "\r\nIf x = 3, then y = ?\r\n", "7", 100);
            }
            else if (activePersons.NoPerson == 2.ToString())
            {
                talkAreas = new TalkAreas("Andy's Room", FinderQuest3D.Properties.Resources.talkArea2, activePersons);
                activePersons.AddQuestion("What is the capital city of Indonesia ?", "Jakarta", 50);
            }
            else if (activePersons.NoPerson == 3.ToString())
            {
                talkAreas = new TalkAreas("Bobby's Office", FinderQuest3D.Properties.Resources.talkArea3, activePersons);
                activePersons.AddQuestion("I have this pattern: \r\n1\t1\t2\t3\t5\t8 ...\r\nWhat is the next number?\r\n", "13", 150);
            }
            else if (activePersons.NoPerson == 4.ToString())
            {
                talkAreas = new TalkAreas("Rina's Room", FinderQuest3D.Properties.Resources.talkArea4, activePersons);
                activePersons.AddQuestion("What is the chemical compound name for sulfuric acid?\r\n", "h2so4", 100);
            }
            else if (activePersons.NoPerson == 5.ToString())
            {
                talkAreas = new TalkAreas("Tommy's Place", FinderQuest3D.Properties.Resources.talkArea5, activePersons);
                activePersons.AddQuestion("Check this C# codes: \r\nint result = 10/100; MessageBox.Show(result);\r\nWhat is the output of these codes?\r\n", "0", 150);
            }
            else if (activePersons.NoPerson == 6.ToString())
            {
                talkAreas = new TalkAreas("Marie's Place", FinderQuest3D.Properties.Resources.talkArea6, activePersons);
                activePersons.AddQuestion("A product has a selling price of $100 and is discounted 10% off the list price. It also has a shipping fee of $5. \r\nIf you want to purchase this product, how much will you have to pay?\r\n", "95", 150);
            }
            else if (activePersons.NoPerson == 7.ToString())
            {
                talkAreas = new TalkAreas("Luke's Home", FinderQuest3D.Properties.Resources.talkArea7, activePersons);
                activePersons.AddQuestion("What is the 1st principle (sila ke - 1) of Pancasila ?\r\n", "Ketuhanan Yang Maha Esa", 50);
            }
            panelGameBottom.Visible = true;
            labelTalkArea.Visible = true;
            talkAreas.DisplayPicture(panelGameBottom);
            labelTalkArea.Text = talkAreas.DisplayData();
            labelTalkArea.BringToFront();
            panelGameBottom.BringToFront();
            if (player != null) player.Picture.Visible = false;
            activePersons.DisplayPicture(panelGameBottom);
            activePersons.Picture.Size = new Size(150, 200);
            activePersons.Picture.Location = new Point(400, 250);
            activePersons.DisplayDialogs(panelGameBottom);
        }

        //private void GenerateWalkArea()
        //{
        //    if (walkAreas == null)
        //    {
        //        walkAreas = new WalkAreas(1, "The Barn", FinderQuest3D.Properties.Resources.walkArea1);
        //        walkAreas.AddPerson(1.ToString(), "Anna", FinderQuest3D.Properties.Resources.person1,
        //            new Size(80, 120), new Point(200, 420), "I have a question for you. Are you ready?");
        //        walkAreas.AddPerson(2.ToString(), "Andy", FinderQuest3D.Properties.Resources.person2,
        //            new Size(80, 120), new Point(500, 420), "Can't you answer the question?. Let's Go!");
        //        walkAreas.AddPerson(3.ToString(), "Bobby", FinderQuest3D.Properties.Resources.person3,
        //            new Size(80, 120), new Point(700, 420), "Answer my question");
        //    }
        //    else if (walkAreas.NoArea == 2)
        //    {
        //        walkAreas = new WalkAreas(2, "The Field", FinderQuest3D.Properties.Resources.walkArea2);
        //        walkAreas.AddPerson(4.ToString(), "Rina", FinderQuest3D.Properties.Resources.person4,
        //            new Size(80, 120), new Point(200, 420), "Please answer my question ... ");
        //        walkAreas.AddPerson(5.ToString(), "Tony", FinderQuest3D.Properties.Resources.person5,
        //            new Size(80, 120), new Point(500, 420), "Answer this true man's question!");
        //    }
        //    else if (walkAreas.NoArea == 3)
        //    {
        //        walkAreas = new WalkAreas(3, "The Farm", FinderQuest3D.Properties.Resources.walkArea3);
        //        walkAreas.AddPerson(6.ToString(), "Marie", FinderQuest3D.Properties.Resources.person6,
        //            new Size(80, 120), new Point(200, 450), "I have a question for you. Are you ready?");
        //        walkAreas.AddPerson(7.ToString(), "Luke", FinderQuest3D.Properties.Resources.person7,
        //            new Size(80, 120), new Point(500, 450), "Can't you answer the question?. Let's Go!");
        //    }
        //    else if (walkAreas.NoArea > 3)
        //    {
        //        PlaySound("win");
        //        MessageBox.Show("Congratulations! You win the game!!!");
        //        GameOver();
        //    }
        //    walkAreas.DisplayPicture(this);
        //    labelArea.Text = walkAreas.DisplayData();
        //    walkAreas.DisplayPerson(this);
        //}

        private void FormMenu_KeyDown(object sender, KeyEventArgs e)
        {
            int distance = 30;
            try
            {
                if (isStart == false || isPause == true)
                {
                    throw new Exception();
                }
                else
                {
                    if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
                    {
                        if (player != null)
                        {
                            player.MoveRight(distance);
                            if (player.Picture.Location.X + player.Picture.Width >= this.Width 
                                && walkAreas.CheckFinishAllQuestion())
                            {
                                walkAreas.NoArea++;
                                walkAreas.RemovePerson();
                                //GenerateWalkArea();
                                player.Picture.Location = new Point(0, player.Picture.Location.Y);
                            }
                        }
                    }
                    else if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
                    {
                        if (player != null) player.MoveLeft(distance);
                    }
                    else if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
                    {
                        if (player != null) player.MoveUp(distance);
                    }
                    else if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
                    {
                        if (player != null) player.MoveDown(distance);
                    }
                    else if (e.KeyCode == Keys.Enter) // For adding dialogues 
                    {
                        if (player != null && walkAreas.CheckTouchPerson(player, out Persons touchPerson) == true) 
                        {
                            activePersons = touchPerson;
                            lastLocation = activePersons.Picture.Location;
                            lastSize = activePersons.Picture.Size;
                            // enter talk area
                            GenerateTalkArea();
                        }
                    }
                    else if (e.KeyCode == Keys.Escape) // For exit conversation
                    {
                        
                    }
                    else if (e.KeyCode == Keys.Y && enterTalkAreas == true
                        && activePersons.SolvedStatus == false) // For answering question
                    {
                        FormQuestion1 form = new FormQuestion1();
                        form.Owner = this;
                        form.ShowDialog();
                    }
                    if (player != null) player.DisplayPicture(this);
                }
            }
            catch { }
        }

        public void ExitTalkArea()
        {
            panelGameBottom.Visible = false;
            enterTalkAreas = false;
            if (player != null) player.Picture.Visible = true;
            activePersons.DisplayPicture(this);
            activePersons.Picture.Size = lastSize;
            activePersons.Picture.Location = lastLocation;
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
            if (isPause == false)
            {
                PauseGame();
            }
            else
            {
                ContinueGame();
            }
        }
    }
}
