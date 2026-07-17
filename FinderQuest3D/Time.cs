using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinderQuest3D
{
    [Serializable]
    public class Time
    {
        private int hour;
        private int minute;
        private int second;

        #region constructors

        //default constructor
        public Time()
        {
            this.Hour = 1;
            this.Minute = 2;
            this.Second = 3;
        }

        //parameterized constructor
        public Time(int hh, int mm, int ss)
        {
            this.Hour = hh;
            this.Minute = mm;
            this.Second = ss;
        }

        #endregion

        #region properties
        public int Hour
        {
            get => hour;
            set
            {
                if (value >= 0 && value <= 23)
                {
                    hour = value;
                }
                else
                {
                    hour = 0;
                }
            }
        }
        public int Minute
        {
            get => minute;
            set
            {
                if (value >= 0 && value <= 59)
                {
                    minute = value;
                }
                else
                {
                    minute = 0;
                }
            }
        }
        public int Second
        {
            get => second;
            set
            {
                if (value >= 0 && value <= 59)
                {
                    second = value;
                }
                else
                {
                    second = 0;
                }
            }
        }
        #endregion

        #region methods
        //Method to convert time to second
        public int ConvertToSecond()
        {
            int totalSecond = this.Hour * 3600 + this.Minute * 60 + this.Second;
            return totalSecond;
        }

        //Method to add time
        public void AddWithSecond(int detikInputan)
        {
            // Convert time to second
            int totalDetikSaatIni = this.ConvertToSecond();
            // Calculate second
            int totalDetik = totalDetikSaatIni + detikInputan;
            // Convert to time
            this.Hour = totalDetik / 3600;
            this.Minute = totalDetik % 3600 / 60;
            this.Second = totalDetik % 3600 % 60;
        }

        // Display data
        public string DisplayData()
        {
            string data = this.Hour.ToString().PadLeft(2, '0') + ":" +
                          this.Minute.ToString().PadLeft(2, '0') + ":" +
                          this.Second.ToString().PadLeft(2, '0');
            return data;
        }

        #endregion
    }
}