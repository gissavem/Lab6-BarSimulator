using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Lab6
{
    class PubSimulator
    {
        private readonly MainWindow mainWindow;
        private Pub pub;

        public PubSimulator(Pub pub, MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.pub = pub;
        }

        public void RunSimulation()
        {
            mainWindow.OpenClosePub = ExitSimulation;
            pub.OpeningTimeStamp = DateTime.Now;
            pub.Open();

        }

        internal void ExitSimulation()
        {
            mainWindow.OpenClosePub = RunSimulation;
            throw new NotImplementedException();
        }
    }
}
