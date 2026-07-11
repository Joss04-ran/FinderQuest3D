using SharpDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FinderQuest3D
{
    public class WalkAreas : Areas
    {
        #region Fields
        private int noArea;
        private List<Persons> listPersons; // Composition and 1 walk area > 1 person
        #endregion
        #region Constructor
        public WalkAreas(int noArea, string name, Image background):base(name, background)
        {
            this.NoArea = noArea;
            this.ListPersons = new List<Persons>();
        }
        #endregion
        #region Properties
        public int NoArea { get => noArea; set => noArea = value; }
        public List<Persons> ListPersons { get => listPersons; private set => listPersons = value; }
        #endregion
        #region Methods
        protected override Mesh BuildMapMesh()
        {
            return base.BuildMapMesh();
        }

        public override string DisplayData()
        {
            string data = this.NoArea + " - " + base.Name;
            return data;
        }

        // method to create new person
        public void AddPerson(string noPerson, string name, Image image, Size size, 
            System.Drawing.Point location, string dialog)
        {
            Persons person = new Persons(noPerson, name, image, size, location, dialog);
            this.listPersons.Add(person);
        }

        public void DisplayPerson(Control container)
        {
            foreach (Persons person in listPersons)
            {
                person.DisplayPicture(container);
                person.Picture.BringToFront();
            }
        }

        public void RemovePerson()
        {
            foreach (Persons person in listPersons)
            {
                person.Picture.Dispose();
            }
            this.listPersons.Clear();
        }

        public bool CheckTouchPerson(Players player, out Persons touchPerson)
        {
            foreach(Persons person in listPersons)
            {
                if (player.Picture.Bounds.IntersectsWith(person.Picture.Bounds))
                {
                    touchPerson = person;
                    return true;
                }
            }
            touchPerson = null;
            return false;
        }

        public bool CheckTouchPerson(Vector3 cameraPosition, out Persons touchPerson)
        {
            float interactionDistance = 20.0f; // distance within which a player can talk to an NPC
            foreach (Persons person in listPersons)
            {
                float dx = cameraPosition.X - person.Position.X;
                float dz = cameraPosition.Z - person.Position.Z;
                float dist = (float)Math.Sqrt(dx * dx + dz * dz);
                if (dist <= interactionDistance)
                {
                    touchPerson = person;
                    return true;
                }
            }
            touchPerson = null;
            return false;
        }

        public bool CheckFinishAllQuestion()
        {
            int numSolved = 0;
            foreach (Persons person in listPersons)
            {
                if (person.SolvedStatus == true)
                {
                    numSolved++;
                }
            }
            if (numSolved == this.ListPersons.Count) return true;
            else return false;
        }
        #endregion
    }
}