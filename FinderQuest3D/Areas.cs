using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FinderQuest3D
{
    public abstract class Areas : Map
    {
        #region Fields
        private string name;
        private Image background;
        #endregion
        #region Constructor
        protected Areas(string name, Image background) : base()
        {
            this.Name = name;
            this.Background = background;
        }
        #endregion
        #region Properties
        public string Name { get => name; set => name = value; }
        public Image Background { get => background; set => background = value; }
        #endregion
        #region Methods
        // Abstract class
        public abstract string DisplayData();

        public void DisplayPicture(Control container)
        {
            container.BackgroundImage = this.Background;
            container.BackgroundImageLayout = ImageLayout.Stretch;
        }

        #endregion
    }
}