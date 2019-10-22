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
        private readonly MainWindow mainWindow;

        public PubSimulator(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void RunSimulation()
        {
            mainWindow.OpenClosePub = ExitSimulation;
            var pub = new Pub(9, 8, 120);
            pub.Initialize();
            pub.Open();

        }

        internal void ExitSimulation()
        {
            mainWindow.OpenClosePub = RunSimulation;
            throw new NotImplementedException();
        }
    }
}
