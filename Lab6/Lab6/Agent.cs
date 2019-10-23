using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    public abstract class Agent
    {
        public Agent(Pub pub, LogHandler logHandler)
        {
            Pub = pub;
            LogHandler = logHandler;
        }

        public Pub Pub { get; }
        public LogHandler LogHandler { get; }

        public abstract void GoHome();

        public abstract void Simulate();

    }

    
}
