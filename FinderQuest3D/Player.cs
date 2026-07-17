using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FinderQuest3D
{
    [Serializable]
    public class Players
    {
        #region Fields
        private string name;

        [NonSerialized]
        private PictureBox picture;

        private int score;
        private Time playTime;
        #endregion

        #region Constructor
        public Players(string name, Image image, Size size, Point location, Time playTime)
        {
            this.Name = name;
            this.Picture = new PictureBox();
            this.Picture.Image = image;
            this.Picture.Size = size;
            this.Picture.Location = location;
            this.Score = 0;
            this.PlayTime = playTime;
        }
        #endregion

        #region Properties
        public string Name 
        { 
            get => name; 
            set
            {
                if (value == "")
                    throw new Exception("Name cannot be empty");
                else name = value;
            }
        }
        public PictureBox Picture { get => picture; private set => picture = value; }
        public int Score 
        { 
            get => score; 
            private set
            {
                if (value >= 0)
                    score = value;
                else score = 0;
            }
        }
        public Time PlayTime { get => playTime; set => playTime = value; }
        #endregion

        #region Methods

        public string DisplayData()
        {
            string data = $"Name = {this.Name}" +
                $"\nScore = {this.Score}" +
                $"\nPlaytime = {this.PlayTime.DisplayData()}";
            return data;
        }

        public void DisplayPicture(Control container)
        {
            this.Picture.Parent = container;
            this.Picture.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Picture.BackColor = Color.Transparent;
            this.Picture.BringToFront();
        }

        public void HidePicture(Control container)
        {
            this.Picture.Parent = container;
            this.Picture.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Picture.BackColor = Color.Transparent;
            this.Picture.Hide();
        }

        public void MoveRight(int distance)
        {
            this.Picture.Location = new Point(this.Picture.Location.X + distance
                , this.Picture.Location.Y);
            this.Picture.Image = FinderQuest3D.Properties.Resources.player_right;
        }

        public void MoveUp(int distance)
        {
            this.Picture.Location = new Point(this.Picture.Location.X
                , this.Picture.Location.Y - distance);
            this.Picture.Image = FinderQuest3D.Properties.Resources.player_back;
        }

        public void MoveDown(int distance)
        {
            this.Picture.Location = new Point(this.Picture.Location.X
                , this.Picture.Location.Y + distance);
            this.Picture.Image = FinderQuest3D.Properties.Resources.player_front;
        }

        public void MoveLeft(int distance)
        {
            this.Picture.Location = new Point(this.Picture.Location.X - distance
                , this.Picture.Location.Y);
            this.Picture.Image = FinderQuest3D.Properties.Resources.player_left;
        }

        public void AddScore(int score)
        {
            this.Score += score; 
        }

        #endregion
    }
}