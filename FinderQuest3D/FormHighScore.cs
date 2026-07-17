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
        public List<Players> highScores = new List<Players>();
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
            foreach (var p in highScores)
            {
                listBoxDisplay.Items.Add(p.DisplayData().Split('\n'));
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
