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
        public void DisplayRankings(string filter)
        {
            listBoxDisplay.Items.Clear();
            int rankCounter = 1;

            for (int i = 0; i < highScores.Count; i++)
            {
                Players p = highScores[i];

                // If filtering for Wins, skip the Losses (and vice versa)
                if (filter == "Win" && p.Status != "Win") continue;
                if (filter == "Lose" && p.Status != "Lose") continue;

                // Clean up the text layout using your existing Player method
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
            if (frmMain.player.Status == "Win")
            {
                MessageBox.Show("Congratulations! You Win!");
            }
            // 1. Set the default selection to "All" so the list populates immediately
            if (comboBoxDisplay.Items.Count > 0)
            {
                comboBoxDisplay.SelectedIndex = 0;
            }
            DisplayRankings("All");
        }

        private void comboBoxDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxDisplay.SelectedItem != null)
            {
                string selectedFilter = comboBoxDisplay.SelectedItem.ToString();
                DisplayRankings(selectedFilter);
            }
        }
    }
}
