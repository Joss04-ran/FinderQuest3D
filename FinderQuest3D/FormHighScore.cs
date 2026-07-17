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

namespace FinderQuest3D
{
    public partial class FormHighScore : Form
    {
        FormRender frmMain;
        public BindingList<Players> highScores = new BindingList<Players>();
        public void DisplayRankings()
        {
            listBoxDisplay.Items.Clear();

            // Loop through the sorted scores using a standard for-loop to calculate the rank index
            for (int i = 0; i < highScores.Count; i++)
            {
                Players p = highScores[i];
                int rank = i + 1; // Index 0 becomes Rank #1, Index 1 becomes Rank #2, etc.

                // Format the text nicely so it lines up beautifully in your list box
                string playerDetails = p.DisplayData().Replace("\n", " | ");
                string rowText = $"#{rank} -> {playerDetails}";

                listBoxDisplay.Items.Add(rowText);
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
                highScores = (BindingList<Players>)formatter.Deserialize(file);
                file.Close();
            }
        }

        private void FormHighScore_Load(object sender, EventArgs e)
        {
            frmMain = (FormRender)this.Owner;
            listBoxDisplay.Items.Clear();
            DisplayRankings();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
