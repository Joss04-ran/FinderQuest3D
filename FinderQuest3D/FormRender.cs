using SharpDX;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace FinderQuest3D
{
    public partial class FormRender : Form
    {
        public Time time;
        Map map;
        FormMenu form;
        public Players player;
        private Device device;
        private Camera camera;
        private World3D world;
        WalkAreas walkAreas = null;
        TalkAreas talkAreas = null;
        public Persons activePersons = null;
        SharpDX.Point lastLocation; // save previous location
        Size lastSize; // save previous size
        int[,] mapGrid;
        WindowsMediaPlayer soundPlayer = new WindowsMediaPlayer();
        private DateTime previousDate = DateTime.Now;
        private Dictionary<Keys, bool> keyStates = new Dictionary<Keys, bool>();
        private double deltaTime = 0.0;
        private Stopwatch frameTimer = new Stopwatch();
        private const double TargetFrameTime = 1000.0 / 60.0;
        private bool isExit = false;
        private Timer renderLoop;

        public FormRender()
        {
            InitializeComponent();
            panelGameBottom.Visible = false;
            this.KeyPreview = true;
            this.Focus();
            this.Activate();
            time = new Time(0, 0, 30);
            player = new Players("Jonathan", 
                Properties.Resources.player_front, new Size(80, 110), 
                new System.Drawing.Point(10, 420), time);
            timerTime.Interval = 1000;
            timerTime.Start();
            this.DoubleBuffered = true;
            frameTimer.Start();
            form = (FormMenu)this.Owner;
            panelDashboard.Visible = true;
            panelDashboard.BringToFront();
            buttonExit.Visible = false;
            buttonExit.Enabled = false;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Escape)
            {
                if (panelGameBottom.Visible)
                {
                    ExitTalkArea();
                    e.Handled = true;
                    return;
                }
                else
                {
                    isExit = true;
                    this.Close();
                    e.Handled = true;
                    return;
                }
            }
            if (e.KeyCode == Keys.Enter && !panelGameBottom.Visible)
            {
                if (walkAreas != null && walkAreas.CheckTouchPerson(camera.Position, out Persons touchPerson))
                {
                    activePersons = touchPerson;
                    GenerateTalkArea();
                    e.Handled = true;
                    return;
                }
            }
            if (e.KeyCode == Keys.Y && panelGameBottom.Visible && activePersons != null && activePersons.SolvedStatus == false)
            {
                FormQuestion1 questionForm = new FormQuestion1();
                questionForm.Owner = this;
                questionForm.ShowDialog();
                e.Handled = true;
                return;
            }
            keyStates[e.KeyCode] = true;
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            keyStates[e.KeyCode] = false;
            base.OnKeyUp(e);
        }

        private void FormRender_Load(object sender, EventArgs e)
        {
            if (player != null)
            {
                labelPlayer.Text = player.DisplayData();
            }
            player.DisplayPicture(panelPlayerProfile);
            player.Picture.Size = panelPlayerProfile.Size;
            player.Picture.Location = new System.Drawing.Point(0, 0);
            device = new Device(this.Handle, this.ClientSize.Width, this.ClientSize.Height);
            camera = new Camera(new Vector3(150.0f, 4.0f, 150.0f), 1.0f);
            world = new World3D();

            // Locate resource files
            string projectPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", ".."));
            soundPlayer.URL = Path.Combine(projectPath, "bin", "Debug", "sound", "BacksoundWalkArea.mp3");

            string treePath = Path.Combine(projectPath, "Resources", "craftpix-net-385863-free-top-down-trees-pixel-art", "PNG", "Assets_separately", "Trees", "Autumn_tree2.png");
            if (!File.Exists(treePath))
                treePath = Path.Combine(projectPath, "Resources", "craftpix-net-385863-free-top-down-trees-pixel-art", "PNG", "Assets_separately", "Trees", "Christmas_tree1.png");

            string personPath = Path.Combine(projectPath, "Resources", "person1.png");
            GenerateWalkArea();
            camera.Position = map.GetPlayerSpawnPosition();
            world.InitializeFromMap(map, treePath, personPath);

            string skyPath = Path.Combine(projectPath, "Resources", "walkArea1.png");
            world.GenerateSky(skyPath);
            renderLoop = new Timer();
            renderLoop.Interval = (int)(1000.0 / 60.0); // 60 FPS
            renderLoop.Tick += RenderLoopTick;
            renderLoop.Start();
        }

        private void RenderLoopTick(object sender, EventArgs e)
        {
            if (this.IsDisposed || isExit) return;
            if (device == null || world == null || camera == null || map == null) return;
            float moveSpeed = 1.5f;
            float rotateSpeed = 0.05f;
            float playerRadius = 4.0f;

            Vector3 displacement = Vector3.Zero;
            if (keyStates.TryGetValue(Keys.W, out bool w) && w)
                displacement += camera.Forward * moveSpeed;
            if (keyStates.TryGetValue(Keys.S, out bool s) && s)
                displacement -= camera.Forward * moveSpeed;
            if (keyStates.TryGetValue(Keys.A, out bool a) && a)
                displacement -= camera.SideWalk * moveSpeed;
            if (keyStates.TryGetValue(Keys.D, out bool d) && d)
                displacement += camera.SideWalk * moveSpeed;

            if (displacement != Vector3.Zero)
            {
                Vector3 nextPos = camera.Position + displacement;
                if (!map.isCollision(nextPos, playerRadius))
                {
                    camera.Position = nextPos;

                    if (map.IsTransitionArea(nextPos))
                    {
                        if (walkAreas != null && walkAreas.CheckFinishAllQuestion())
                        {
                            walkAreas.NoArea++;
                            GenerateWalkArea();

                            string projectPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", ".."));
                            string treePath = Path.Combine(projectPath, "Resources", "craftpix-net-385863-free-top-down-trees-pixel-art", "PNG", "Assets_separately", "Trees", "Autumn_tree2.png");
                            if (!File.Exists(treePath))
                                treePath = Path.Combine(projectPath, "Resources", "craftpix-net-385863-free-top-down-trees-pixel-art", "PNG", "Assets_separately", "Trees", "Christmas_tree1.png");
                            string personPath = Path.Combine(projectPath, "Resources", "person1.png");

                            if (walkAreas.NoArea <= 3)
                            {
                                world.InitializeFromMap(map, treePath, personPath);
                                camera.Position = map.GetPlayerSpawnPosition();
                            }
                        }
                    }
                }
            }
            if (keyStates.TryGetValue(Keys.Left, out bool left) && left)
                camera.Yaw -= rotateSpeed;
            if (keyStates.TryGetValue(Keys.Right, out bool right) && right)
                camera.Yaw += rotateSpeed;
            world.Update(camera);
            // Render frame
            device.Clear(SharpDX.Color.CornflowerBlue);
            world.Render(device, camera);
            device.Present();
            frameTimer.Restart();
            double elapsedMs = frameTimer.Elapsed.TotalMilliseconds;
            if (elapsedMs < TargetFrameTime)
            {
                System.Threading.Thread.Sleep((int)(TargetFrameTime - elapsedMs));
            }
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

        private void GenerateWalkArea()
        {
            if (walkAreas == null)
            {
                mapGrid = new int[,]{
                {0, 5, 5, 5, 5, 5, 7, 5, 5, 5, 5, 5, 5, 5, 0},
                {5, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 5},
                {5, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 5},
                {5, 2, 2, 2, 2, 2, 1, 6, 2, 2, 2, 2, 2, 2, 5},
                {5, 3, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 5},
                {5, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 5},
                {5, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 5},
                {5, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 5},
                {5, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 5},
                {5, 2, 2, 6, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 5},
                {5, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 5},
                {5, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 5},
                {5, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 6, 5},
                {5, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 5},
                {0, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 0}
            };
                walkAreas = new WalkAreas(1, "The Barn", FinderQuest3D.Properties.Resources.walkArea1);
                walkAreas.AddMap(mapGrid);
                map = walkAreas;
                walkAreas.AddPerson(1.ToString(), "Anna", FinderQuest3D.Properties.Resources.person1,
                    new Size(80, 120), new System.Drawing.Point(200, 420), "I have a question for you. Are you ready?");
                walkAreas.AddPerson(2.ToString(), "Andy", FinderQuest3D.Properties.Resources.person2,
                    new Size(80, 120), new System.Drawing.Point(500, 420), "Can't you answer the question?. Let's Go!");
                walkAreas.AddPerson(3.ToString(), "Bobby", FinderQuest3D.Properties.Resources.person3,
                    new Size(80, 120), new System.Drawing.Point(700, 420), "Answer my question");
            }
            else if (walkAreas.NoArea == 2)
            {
                mapGrid = new int[,]{
                {0, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 0},
                {5, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 5},
                {5, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5},
                {5, 1, 0, 1, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 5},
                {5, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 5},
                {5, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 5},
                {5, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 5},
                {5, 3, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 7},
                {5, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 5},
                {5, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 5},
                {5, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 2, 5},
                {5, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 5},
                {5, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 6, 5},
                {5, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5},
                {0, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 0}
            };
                walkAreas = new WalkAreas(2, "The Field", FinderQuest3D.Properties.Resources.walkArea2);
                walkAreas.AddMap(mapGrid);
                map = walkAreas;
                walkAreas.AddPerson(4.ToString(), "Rina", FinderQuest3D.Properties.Resources.person4,
                    new Size(80, 120), new System.Drawing.Point(200, 420), "Please answer my question ... ");
                walkAreas.AddPerson(5.ToString(), "Tony", FinderQuest3D.Properties.Resources.person5,
                    new Size(80, 120), new System.Drawing.Point(500, 420), "Answer this true man's question!");
            }
            else if (walkAreas.NoArea == 3)
            {
                mapGrid = new int[,]{
                {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5},
                {5, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 5},
                {5, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5},
                {5, 1, 0, 1, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 5},
                {5, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 5},
                {5, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 5},
                {5, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 7},
                {5, 3, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 5},
                {5, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 5},
                {5, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 5},
                {5, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 2, 5},
                {5, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 5},
                {5, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 6, 5},
                {5, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5},
                {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5}
            };
                walkAreas = new WalkAreas(3, "The Farm", FinderQuest3D.Properties.Resources.walkArea3);
                walkAreas.AddMap(mapGrid);
                map = walkAreas;
                walkAreas.AddPerson(6.ToString(), "Marie", FinderQuest3D.Properties.Resources.person6,
                    new Size(80, 120), new System.Drawing.Point(200, 450), "I have a question for you. Are you ready?");
                walkAreas.AddPerson(7.ToString(), "Luke", FinderQuest3D.Properties.Resources.person7,
                    new Size(80, 120), new System.Drawing.Point(500, 450), "Can't you answer the question?. Let's Go!");
            }
            else if (walkAreas.NoArea > 3)
            {
                GameOver();
            }
            walkAreas.DisplayPicture(this);
            labelArea.Text = walkAreas.DisplayData();
            walkAreas.DisplayPerson(this);
            foreach (var person in walkAreas.ListPersons)
            {
                person.Picture.Visible = false;
            }
        }

        public void ExitTalkArea()
        {
            panelGameBottom.Visible = false;
            labelTalkArea.Visible = false;
            if (activePersons != null)
            {
                activePersons.Picture.Visible = false;
            }
            PlaySound("walk");
        }

        private void GenerateTalkArea()
        {
            PlaySound("talk");
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
            activePersons.DisplayPicture(panelGameBottom);
            activePersons.Picture.Visible = true; 
            activePersons.Picture.Size = new Size(250, 400);
            activePersons.Picture.Location = new System.Drawing.Point(350, 200);
            activePersons.DisplayDialogs(panelGameBottom);
        }

        private void timerTime_Tick(object sender, EventArgs e)
        {
            time.AddWithSecond(-1);
            labelTime.Text = time.DisplayData();
            if (player != null)
            {
                labelPlayer.Text = player.DisplayData();
            }
            try
            {
                soundPlayer.controls.play();
            }
            catch { }
            if (time.Hour == 0 && time.Minute == 0 && time.Second == 0)
            {
                timerTime.Stop();
                try
                {
                    soundPlayer.controls.stop();
                    soundPlayer.close();
                }
                catch { }
                PlaySound("lose");
                MessageBox.Show("Game Over! Time's Up!");
                this.Close();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            isExit = true;
            base.OnFormClosing(e);
            timerTime.Stop();
            try
            {
                soundPlayer.controls.stop();
                soundPlayer.close();
            }
            catch { }
            if (world != null)
            {
                try { world.Sky?.Dispose(); } catch { }
                try { world.Floor?.Dispose(); } catch { }
                if (world.Billboards != null)
                {
                    foreach (var billboard in world.Billboards)
                    {
                        try { billboard?.Dispose(); } catch { }
                    }
                }
            }
            try
            {
                device?.Dispose();
            }
            catch { }
            //Environment.Exit(0);
        }

        public void GameOver()
        {
            PlaySound("win");
            MessageBox.Show("Congratulations! You win the game!!!");
            this.Close();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void labelPlayer_Click(object sender, EventArgs e)
        {
            // Empty
        }
    }
}
