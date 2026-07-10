using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinderQuest3D
{
    public class Questions
    {
        #region Fields
        private string question;
        private string answer;
        private int score;
        #endregion
        #region Constructor
        public Questions(string question, string answer, int score)
        {
            this.Question = question;
            this.Answer = answer;
            this.Score = score;
        }
        #endregion
        #region Properties
        public string Question { get => question; set => question = value; }
        public string Answer { get => answer; set => answer = value; }
        public int Score 
        { 
            get => score; 
            set
            { 
                if (value < 0) score = 0;
                else score = value;
            }
        }
        #endregion
    }
}