using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
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
        Time time;
        Map map;
        FormMenu form;
        private Device device;
        private Camera camera;
        private World3D world;
        WindowsMediaPlayer soundPlayer = new WindowsMediaPlayer();
        private DateTime previousDate = DateTime.Now;
        private Dictionary<Keys, bool> keyStates = new Dictionary<Keys, bool>();
        private double deltaTime = 0.0;
        private Stopwatch frameTimer = new Stopwatch();
        private const double TargetFrameTime = 1000.0 / 60.0;
        private bool isExit = false;

        public FormRender()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.Focus();
            this.Activate();
            time = new Time(0, 10, 0);
            timerTime.Interval = 1000;
            timerTime.Start();
            this.DoubleBuffered = true;
            frameTimer.Start();
            form = (FormMenu)this.Owner;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Escape)
            {
                isExit = true;
                this.Close();
                e.Handled = true;
            }
            keyStates[e.KeyCode] = true;
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            keyStates[e.KeyCode] = false;
            base.OnKeyUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Calculate FPS 
            var now = DateTime.Now;
            var elapsed = (now - previousDate).TotalMilliseconds;
            previousDate = now;
            deltaTime = elapsed / 1000.0;
            double fps = elapsed > 0 ? (1000.0 / elapsed) : 0.0;
            this.Text = string.Format("{0:0.00} fps", fps);
        }

        private void FormRender_Load(object sender, EventArgs e)
        {
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

            // Define procedural map grid (maze/custom design)
            int[,] proceduralGrid = {
                {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5},
                {5, 1, 1, 1, 5, 0, 0, 0, 0, 0, 5, 0, 0, 2, 5},
                {5, 1, 5, 1, 5, 0, 5, 5, 5, 0, 5, 0, 5, 0, 5},
                {5, 1, 5, 1, 0, 0, 5, 3, 5, 0, 0, 0, 5, 0, 5},
                {5, 1, 5, 5, 5, 5, 5, 0, 5, 5, 5, 5, 5, 0, 5},
                {5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 5},
                {5, 5, 5, 5, 5, 0, 5, 5, 5, 5, 5, 0, 5, 0, 5},
                {5, 2, 0, 0, 5, 0, 5, 1, 1, 1, 5, 0, 5, 0, 5},
                {5, 0, 5, 0, 5, 0, 5, 1, 5, 1, 5, 0, 0, 0, 5},
                {5, 0, 5, 3, 5, 0, 5, 1, 5, 1, 5, 5, 5, 5, 5},
                {5, 0, 5, 5, 5, 0, 5, 1, 5, 1, 0, 0, 0, 2, 5},
                {5, 0, 0, 0, 0, 0, 5, 1, 5, 5, 5, 5, 5, 0, 5},
                {5, 5, 5, 5, 5, 5, 5, 1, 1, 1, 1, 1, 1, 6, 5},
                {5, 2, 0, 0, 0, 0, 0, 0, 5, 5, 5, 5, 5, 5, 5},
                {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5}
            };
            map = new Map(proceduralGrid);
            world.InitializeFromMap(map, treePath, personPath);

            string skyPath = Path.Combine(projectPath, "Resources", "walkArea1.png");
            world.GenerateSky(skyPath);
            SharpDX.Windows.RenderLoop.Run(this, () =>
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
            });
        }

        private void timerTime_Tick(object sender, EventArgs e)
        {
            time.AddWithSecond(-1);
            labelTime.Text = time.DisplayData();
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
                form.PlaySound("lose");
                MessageBox.Show("Game Over! Time's Up!");
                Application.Exit();
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
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Environment.Exit(0);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
