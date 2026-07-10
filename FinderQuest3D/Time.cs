using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinderQuest3D
{
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
                //nilai jam hrs bernilai 0-23
                if (value >= 0 && value <= 23)
                {
                    hour = value;
                }
                else
                {
                    throw new Exception("Hour must be 0-23");
                }
            }
        }
        public int Minute
        {
            get => minute;
            set
            {
                //nilai menit harus 0-59
                if (value >= 0 && value <= 59)
                {
                    minute = value;
                }
                else
                {
                    throw new Exception("Minute must be 0-59");
                }
            }
        }
        public int Second
        {
            get => second;
            set
            {
                //nilai detik harus 0-59
                if (value >= 0 && value <= 59)
                {
                    second = value;
                }
                else
                {
                    throw new Exception("Second must be 0-59");
                }
            }
        }
        #endregion

        #region methods
        //method utk mengkonversi waktu saat ini (hh:mm:ss) ke detik semua
        public int ConvertToSecond()
        {
            int totalSecond = this.Hour * 3600 + this.Minute * 60 + this.Second;
            return totalSecond;
        }

        //method utk menambah waktu saat ini (hh:mm:ss) dengan detik
        public void AddWithSecond(int detikInputan)
        {
            //1. Konversikan waktu saat ini ke detik semua
            //(gunakan method ConvertToSecond)
            int totalDetikSaatIni = this.ConvertToSecond();
            //2. Tambahkan total detik saat ini dengan detik yg diinputkan user
            int totalDetik = totalDetikSaatIni + detikInputan;
            //3. Konversikan total detik (hasil langkah2) ke hh:mm:ss
            this.Hour = totalDetik / 3600;
            this.Minute = totalDetik % 3600 / 60;
            this.Second = totalDetik % 3600 % 60;
        }

        //method utk menampilkan data waktu dalam format hh:mm:ss
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