using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    class Bouncer : Agent
    {
        private readonly Pub pub;

        //ITS ME, BLACKSMITH

        public Bouncer(Pub pub):base(pub)
        {
            this.pub = pub;
            pub.Close += GoHome;
        }

        public Patron LetPatronInside(Func<string> CheckID)
        {
            return new Patron(CheckID(), pub);
        }

        private string CheckID()
        {
            return "Lenart bladh";
        }
    }
}
