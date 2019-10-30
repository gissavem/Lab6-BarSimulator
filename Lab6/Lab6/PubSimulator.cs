using System;

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
            mainWindow.LabelTimer.Start();
        }
        internal void ExitSimulation()
        {
            Environment.Exit(Environment.ExitCode);
        }
    }
}
