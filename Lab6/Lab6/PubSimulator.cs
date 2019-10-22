using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab6
{
    class PubSimulator
    {

        public PubSimulator()
        {
        }
        public bool IsSimulating { get; set; }

        public void RunSimulation()
        {
            IsSimulating = true;
            var pub = new Pub(9, 8, 120);
            pub.Initialize();
            pub.Open();

        }

        internal void ExitSimulation()
        {
            throw new NotImplementedException();
        }
    }
}
