using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    class Bouncer : Agent
    {
        //ITS ME, BLACKSMITH

        public Bouncer(Pub pub):base(pub)
        {

        }

        public Patron LetPatronInside(Func<string> CheckID)
        {
            return new Patron(CheckID());
        }

        private string CheckID()
        {
            return "Lenart bladh";
        }
    }
}
