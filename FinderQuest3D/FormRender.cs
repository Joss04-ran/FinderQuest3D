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
        public Time playTime;
        Map map;
        FormGameStart form;
        public Players player;
        private Device device;
        private Camera camera;
        private World3D world;
        WalkAreas walkAreas = null;
        TalkAreas talkAreas = null;
        public Persons activePersons = null;
        //SharpDX.Point lastLocation; // save previous location
        //Size lastSize; // save previous size
        int[,] mapGrid;
        WindowsMediaPlayer soundPlayer;
        private DateTime previousDate = DateTime.Now;
        private Dictionary<Keys, bool> keyStates = new Dictionary<Keys, bool>();
        //private double deltaTime = 0.0;
        private Stopwatch frameTimer = new Stopwatch();
        private const double TargetFrameTime = 1000.0 / 60.0;
        private bool isExit = false;
        private bool isComplete = false;
        private Timer renderLoop;
        public string filePath = "highscore.dat";
        public bool isPaused = false;
        public bool isMuted = false;

        public FormRender(FormGameStart ownerForm)
        {
            InitializeComponent();
            soundPlayer = new WindowsMediaPlayer();
            this.Owner = ownerForm;
            form = ownerForm;
            panelGameBottom.Visible = false;
            this.KeyPreview = true;
            this.Focus();
            this.Activate();
            if (form.difficult == "easy") time = new Time(0, 5, 0);
            else if (form.difficult == "hard") time = new Time(0, 1, 0);
            playTime = new Time(0, 0, 0);
            player = new Players(form.name, 
                Properties.Resources.player_front, new Size(80, 110), 
                new System.Drawing.Point(10, 420), playTime);
            timerTime.Interval = 1000;
            timerTime.Start();
            this.DoubleBuffered = true;
            frameTimer.Start();
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
                if (form.difficult == "easy")
                {
                    FormQuestionEasy questionForm = new FormQuestionEasy();
                    questionForm.Owner = this;
                    questionForm.ShowDialog();
                    e.Handled = true;
                    return;
                }
                else if (form.difficult == "hard")
                {
                    FormQuestionHard questionForm = new FormQuestionHard();
                    questionForm.Owner = this;
                    questionForm.ShowDialog();
                    e.Handled = true;
                    return;
                }
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
            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
            PlaySound("main");

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

                            string projectPath = AppDomain.CurrentDomain.BaseDirectory;
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

        public void PauseGame()
        {
            timerTime.Stop();
            if (renderLoop != null) renderLoop.Stop();
            labelArea.Text = "Paused";
        }

        public void Continue()
        {
            timerTime.Start();
            if (renderLoop != null) renderLoop.Start();
            if (walkAreas != null)
            {
                labelArea.Text = walkAreas.DisplayData();
            }
            else
            {
                labelArea.Text = "";
            }
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
                {5, 1, 1, 6, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 5},
                {5, 1, 2, 1, 2, 2, 1, 2, 1, 1, 1, 1, 1, 1, 5},
                {5, 1, 2, 1, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1, 5},
                {5, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 7},
                {5, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 5},
                {5, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 5},
                {5, 3, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 1, 5},
                {5, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 1, 5},
                {5, 2, 6, 2, 1, 1, 1, 1, 2, 2, 1, 2, 2, 1, 5},
                {5, 2, 1, 2, 2, 2, 2, 1, 2, 2, 1, 2, 2, 1, 5},
                {5, 2, 1, 2, 2, 2, 2, 1, 2, 2, 1, 2, 2, 1, 5},
                {5, 2, 1, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 5},
                {5, 2, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 5},
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
                {0, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 0},
                {5, 2, 2, 2, 2, 2, 2, 6, 2, 2, 2, 2, 2, 2, 5},
                {5, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 5},
                {5, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 5},
                {5, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5},
                {5, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 5},
                {5, 2, 2, 1, 1, 6, 2, 2, 2, 2, 2, 2, 2, 1, 5},
                {5, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 1, 5},
                {5, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 1, 5},
                {5, 2, 2, 1, 1, 1, 2, 1, 2, 2, 1, 2, 2, 1, 5},
                {5, 2, 2, 1, 2, 2, 2, 1, 2, 2, 1, 2, 2, 1, 5},
                {5, 2, 2, 1, 1, 1, 2, 1, 2, 2, 1, 2, 2, 1, 5},
                {5, 2, 2, 1, 2, 1, 2, 1, 1, 1, 1, 1, 1, 1, 5},
                {5, 2, 1, 1, 2, 1, 1, 1, 2, 2, 2, 2, 2, 1, 5},
                {0, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 7, 0}
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
        }

        private void GenerateTalkArea()
        {
            if (activePersons.NoPerson == 1.ToString())
            {
                talkAreas = new TalkAreas("Anna's House", FinderQuest3D.Properties.Resources.talkArea1, activePersons);
                activePersons.AddQuestion("Solve this math equation: " +
                    "\r\n x + y = 10 " +
                    "\r\nIf x = 3, then y = ?\r\n", "7", 100);
                activePersons.AddQuestion("What is the x in x^2+10x+25=0? [+/-]", "5",100);
                activePersons.AddQuestion("If x+2y = 10 and y=5, what is x=?","0", 50);
                if (form.difficult == "hard")
                {
                    activePersons.AddQuestion("What is the default assumption in Hypothesis testing called?", "H0", 100);
                    activePersons.AddQuestion("The time a person waits for a plane at an airport ranges from 0 to 120 " +
                        "minutes and is\nuniformly distributed.\nWhat is the probability that a person will have to wait less than 60 minutes?", "3/4", 200);
                }
            }
            else if (activePersons.NoPerson == 2.ToString())
            {
                talkAreas = new TalkAreas("Andy's Room", FinderQuest3D.Properties.Resources.talkArea2, activePersons);
                activePersons.AddQuestion("What is the capital city of Indonesia ?", "Jakarta", 50);
                activePersons.AddQuestion("What is the capital city of Japan?", "Tokyo", 100);
                activePersons.AddQuestion("What is the capital city of China?", "Beijing", 100);
                if (form.difficult == "hard")
                {
                    activePersons.AddQuestion("What is the longest river in the world?", "Nile River", 100);
                    activePersons.AddQuestion("How many states are there in the United States of America?", "50", 100);
                }
            }
            else if (activePersons.NoPerson == 3.ToString())
            {
                talkAreas = new TalkAreas("Bobby's Office", FinderQuest3D.Properties.Resources.talkArea3, activePersons);
                activePersons.AddQuestion("I have this pattern: \r\n1\t1\t2\t3\t5\t8 ...\r\nWhat is the next number?\r\n", "13", 150);
                activePersons.AddQuestion("What is the value of 10 factorial divided by 9 factorial (10! / 9!)?", "10", 100);
                activePersons.AddQuestion("If a circle's radius is 10, what is its diameter?", "20", 100);
                if (form.difficult == "hard")
                {
                    activePersons.AddQuestion("The derivative of y=3x^2+x+e^2", "6x+1", 200);
                    activePersons.AddQuestion("The integral of 2x+9", "x^2+9x", 150);
                }
            }
            else if (activePersons.NoPerson == 4.ToString())
            {
                talkAreas = new TalkAreas("Rina's Room", FinderQuest3D.Properties.Resources.talkArea4, activePersons);
                activePersons.AddQuestion("What is the chemical compound name for sulfuric acid?\r\n", "h2so4", 100);
                activePersons.AddQuestion("Octopuses have how many hearts?", "3", 100);
                activePersons.AddQuestion("Chromosome Sex for Male are …", "XY", 150);
                if (form.difficult == "hard")
                {
                    activePersons.AddQuestion("How many is a Human Chromosome?", "46", 100);
                    activePersons.AddQuestion("What is the name of the process in which a solid changes directly into a gas?", "Sublimation", 100);
                }
            }
            else if (activePersons.NoPerson == 5.ToString())
            {
                talkAreas = new TalkAreas("Tommy's Place", FinderQuest3D.Properties.Resources.talkArea5, activePersons);
                activePersons.AddQuestion("Check this C# codes: \r\nint result = 10/100; MessageBox.Show(result);\r\nWhat is the output of these codes?\r\n", "0", 150);
                activePersons.AddQuestion("What is aggregation has-a … relationship", "Weak", 100);
                activePersons.AddQuestion("What is the open source application for web server?", "Apache", 150);
                if (form.difficult == "hard")
                {
                    activePersons.AddQuestion("What is antiX based on?", "Debian", 100);
                    activePersons.AddQuestion("What is the L in LAMP Stack open source software?", "Linux", 200);
                }
            }
            else if (activePersons.NoPerson == 6.ToString())
            {
                talkAreas = new TalkAreas("Marie's Place", FinderQuest3D.Properties.Resources.talkArea6, activePersons);
                activePersons.AddQuestion("A product has a selling price of $100 and is discounted " +
                    "10% off the list price. It also has a shipping fee of $5. \r\nIf you want to purchase this product, " +
                    "how much will you have to pay?\r\n", "95", 150);
                activePersons.AddQuestion("What does GDP stand for?", "Gross Domestic Product", 100);
                activePersons.AddQuestion("A sustained increase in the general price level of goods and services, is called?", "Inflation", 100);
                if (form.difficult == "hard")
                {
                    activePersons.AddQuestion("What is the currency of Taiwan?[Simplified]", "NTD", 100);
                    activePersons.AddQuestion("What is the name of the curve that shows the relationship between inflation and unemployment?", 
                        "Phillips Curve", 100);
                }
            }
            else if (activePersons.NoPerson == 7.ToString())
            {
                talkAreas = new TalkAreas("Luke's Home", FinderQuest3D.Properties.Resources.talkArea7, activePersons);
                activePersons.AddQuestion("What is the 1st principle (sila ke - 1) of Pancasila ?\r\n", "Ketuhanan Yang Maha Esa", 50);
                activePersons.AddQuestion("Who is the first president of Indonesia?", "Sukarno", 100);
                activePersons.AddQuestion("What is the motto of Indonesia?", "Bhinneka Tunggal Ika", 50);
                if (form.difficult == "hard")
                {
                    activePersons.AddQuestion("Which institution organizes " +
                        "general elections in Indonesia?", "KPU", 200);
                    activePersons.AddQuestion("How many feathers are on " +
                        "each wing of Garuda Pancasila?", "17", 200);
                }
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
            playTime.AddWithSecond(1);
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
            if (time.Hour == 0 && time.Minute == 0 && time.Second > 30)
            {
                labelTime.BackColor = System.Drawing.Color.Transparent;
            }
            if (time.Hour == 0 && time.Minute == 0 && time.Second <= 30)
            {
                labelTime.BackColor = System.Drawing.Color.Orange;
            }
            if (time.Hour == 0 && time.Minute == 0 && time.Second <= 10)
            {
                labelTime.BackColor = System.Drawing.Color.DarkRed;
            }
            if (time.Hour == 0 && time.Minute == 0 && time.Second == 0)
            {
                timerTime.Stop();
                if (renderLoop != null) renderLoop.Stop();
                try
                {
                    soundPlayer.controls.stop();
                    soundPlayer.close();
                }
                catch { }
                PlaySound("lose");
                player.Status = "Lose";
                FormHighScore formHighScore = new FormHighScore();
                formHighScore.Owner = this;
                formHighScore.ReadData(filePath); // 1. READ HISTORY FIRST
                formHighScore.highScores.Add(player);
                formHighScore.highScores.Sort((p1, p2) => p2.Score.CompareTo(p1.Score));// 3. SORT BY HIGHEST SCORE
                formHighScore.SaveData(filePath);
                formHighScore.ShowDialog();
                // --- FIX: Safely find and close open sub-forms ---
                List<Form> openForms = new List<Form>(Application.OpenForms.Cast<Form>());
                foreach (Form openForm in openForms)
                {
                    if (openForm is FormQuestionEasy || openForm is FormQuestionHard)
                    {
                        openForm.Close();
                    }
                }
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
            if (isComplete) return;
            isComplete = true;
            if (renderLoop != null) renderLoop.Stop();
            if (timerTime != null) timerTime.Stop();
            try
            {
                if (isComplete)
                {
                    PlaySound("win");
                    player.Status = "Win";
                    FormHighScore formHighScore = new FormHighScore();
                    formHighScore.Owner = this;
                    formHighScore.ReadData(filePath); 
                    formHighScore.highScores.Add(player);
                    formHighScore.highScores.Sort((p1, p2) => p2.Score.CompareTo(p1.Score));
                    formHighScore.SaveData(filePath);
                    formHighScore.ShowDialog();
                    // --- ADDED FOR WIN CLEANUP: Safely close open sub-forms ---
                    List<Form> openForms = new List<Form>(Application.OpenForms.Cast<Form>());
                    foreach (Form openForm in openForms)
                    {
                        if (openForm is FormQuestionEasy || openForm is FormQuestionHard)
                        {
                            openForm.Close();
                        }
                    }
                    this.Close();
                }
            }
            catch { }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void labelPlayer_Click(object sender, EventArgs e)
        {
            // Empty
        }

        private void toolStripMenuItemPlayPause_Click(object sender, EventArgs e)
        {
            if (isPaused == false)
            {
                PauseGame();
                soundPlayer.settings.mute = !soundPlayer.settings.mute;
                isPaused = true;
            }
            else if (isPaused == true)
            {
                Continue();
                soundPlayer.settings.mute = false;
                isPaused = false;
            }
        }

        private void toolStripMenuItemMuteUnmute_Click(object sender, EventArgs e)
        {
            if (isMuted == false)
            {
                soundPlayer.settings.mute = !soundPlayer.settings.mute;
                isMuted = true;
            }
            else if (isMuted == true)
            {
                soundPlayer.settings.mute = false;
                isMuted = false;
            }
        }
    }
}



