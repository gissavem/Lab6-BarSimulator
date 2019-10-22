using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    public abstract class Agent
    {
        public Agent(Pub pub)
        {
            Pub = pub;
        }

        public Pub Pub { get; }

        public void GoHome()
        {
            //TODO : Implement
        }
    }

    
}
