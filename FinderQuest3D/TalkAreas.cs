using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FinderQuest3D
{
    public class TalkAreas : Areas
    {
        private Persons person; // Aggregation and 1 talk area = 1 person

        public TalkAreas(string name, Image background, Persons person)
            :base(name, background)
        {
            this.Person = person;
        }

        public Persons Person { get => person; set => person = value; }

        public override string DisplayData()
        {
            return base.Name;
        }
    }
}