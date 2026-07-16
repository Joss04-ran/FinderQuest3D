using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FinderQuest3D
{
    public class Persons
    {
        #region Fields
        private string noPerson;
        private string name;
        private PictureBox picture;
        private string dialog;
        private bool solvedStatus;
        private List<Questions> personQuestion; // Composition
        private Vector3 position;
        #endregion

        #region Constructor
        public Persons(string noPerson, string name, 
            Image image, Size size, System.Drawing.Point location, string dialog)
        {
            this.NoPerson = noPerson;
            this.Name = name;
            this.Picture = new PictureBox();
            this.Picture.Image = image;
            this.Picture.Location = location;
            this.Picture.Size = size;
            this.Dialog = dialog;
            this.SolvedStatus = false;
            this.PersonQuestion = new List<Questions>();
        }
        #endregion

        #region Properties
        public string NoPerson { get => noPerson; set => noPerson = value; }
        public string Name { get => name; set => name = value; }
        public PictureBox Picture { get => picture; set => picture = value; }
        public string Dialog { get => dialog; set => dialog = value; }
        public bool SolvedStatus { get => solvedStatus; set => solvedStatus = value; }
        public List<Questions> PersonQuestion { get => personQuestion; private set => personQuestion = value; }
        public Vector3 Position { get => position; set => position = value; }
        #endregion

        #region Methods
        public void AddQuestion(string question, string answer, int score)
        {
            Questions questions = new Questions(question, answer, score);
            personQuestion.Add(questions);
        }
        public string DisplayData()
        {
            string data = $"Hi.. I'm {this.Name}";
            return data;
        }
        public void DisplayPicture(Control container)
        {
            this.Picture.Parent = container;
            this.Picture.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Picture.BackColor = System.Drawing.Color.Transparent;
            this.Picture.BringToFront();
        }
        public void DisplayDialogs(Control container)
        {
            Label labelDialog = new Label();
            labelDialog.Parent = container;
            if (this.SolvedStatus == false)
                labelDialog.Text = this.DisplayData() + "\n" + this.Dialog + "\nPress Y to continue";
            else
                labelDialog.Text = this.DisplayData() + "\nCongratulations!\nYou have answered my question";
            labelDialog.AutoSize = false;
            labelDialog.BackColor = System.Drawing.Color.FromArgb(255, 255, 193);
            labelDialog.Size = new Size(400,200);
            labelDialog.Font = new Font("Lucida Sans Typewriter", 18F, FontStyle.Bold);
            labelDialog.Location = new System.Drawing.Point(250,11);
            labelDialog.TextAlign = ContentAlignment.MiddleCenter;
            labelDialog.BorderStyle = BorderStyle.FixedSingle;
            labelDialog.BringToFront();
        }

        public bool CheckAnswer(string playerAnswer, int amount)
        {
            if (playerAnswer.ToLower() == this.PersonQuestion[amount].Answer.ToLower())
            {
                return true;
            }
            else 
                return false;
        }
        public bool IsFinish()
        {
            foreach (Questions q in this.PersonQuestion)
            {
                // If even one question is NOT correct yet, the NPC tasks are not finished
                if (q.Status != "V")
                {
                    this.SolvedStatus = false;
                    return false;
                }
            }
            // If the loop finishes without finding a non-"V" status, they passed everything!
            this.SolvedStatus = true;
            return true;
        }
        #endregion
    }
}