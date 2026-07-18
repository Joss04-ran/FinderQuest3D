using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FinderQuest3D
{
    public partial class FormHighScore : Form
    {
        FormRender frmMain;
        public List<Players> highScores = new List<Players>();
        public void DisplayRankings()
        {
            listBoxDisplay.Items.Clear();
            int rankCounter = 1;

            string statusFilter = "All";
            if (comboBoxDisplay.SelectedItem != null)
            {
                statusFilter = comboBoxDisplay.SelectedItem.ToString();
            }

            string timeFilter = "All";
            if (comboBoxDisplayTime.SelectedItem != null)
            {
                timeFilter = comboBoxDisplayTime.SelectedItem.ToString();
            }

            var filteredScores = highScores.Where(p => 
                statusFilter == "All" || 
                (statusFilter == "Win" && p.Status == "Win") || 
                (statusFilter == "Lose" && p.Status == "Lose")
            ).ToList();

            if (timeFilter == "Time : Lowest To Highest")
            {
                filteredScores = filteredScores.OrderBy(p => p.PlayTime.ConvertToSecond()).ToList();
            }
            else if (timeFilter == "Time : Highest To Lowest")
            {
                filteredScores = filteredScores.OrderByDescending(p => p.PlayTime.ConvertToSecond()).ToList();
            }
            foreach (var p in filteredScores)
            {
                string playerDetails = p.DisplayData().Replace("\n", " | ");
                string rowText = $"#{rankCounter} -> {playerDetails}";

                listBoxDisplay.Items.Add(rowText);
                rankCounter++;
            }
        }
        public FormHighScore()
        {
            InitializeComponent();
        }
        public void SaveData(string fileName)
        {
            FileStream file = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(file, highScores);
            file.Close();
        }
        public void ReadData(string fileName)
        {
            if (File.Exists(fileName))
            {
                FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryFormatter formatter = new BinaryFormatter();
                highScores = (List<Players>)formatter.Deserialize(file);
                file.Close();
            }
        }

        private void FormHighScore_Load(object sender, EventArgs e)
        {
            frmMain = (FormRender)this.Owner;
            listBoxDisplay.Items.Clear();
            try
            {
                if (frmMain.player.Status == "Win")
                {
                    MessageBox.Show("Congratulations! You Win!");
                }
                else if (frmMain.player.Status == "Lose")
                {
                    MessageBox.Show("Game Over! Time's Up!");
                }
            }
            catch { }
            if (comboBoxDisplay.Items.Count > 0 && comboBoxDisplay.SelectedIndex == -1)
            {
                comboBoxDisplay.SelectedIndex = 0;
            }
            if (comboBoxDisplayTime.Items.Count > 0 && comboBoxDisplayTime.SelectedIndex == -1)
            {
                comboBoxDisplayTime.SelectedIndex = 0;
            }
            DisplayRankings();
        }

        private void comboBoxDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayRankings();
        }

        private void comboBoxDisplayTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayRankings();
        }
    }
}
