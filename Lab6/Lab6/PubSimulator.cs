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

        public PubSimulator(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            Bouncer.PatronEnters += OnPatronEnters;
        }

        public void RunSimulation()
        {
            mainWindow.OpenClosePub = ExitSimulation;
            pub = new Pub(9, 8, 120);
            pub.Initialize();
            pub.Open();

        }

        public void UpdateLabels()
        {
            mainWindow.Dispatcher.Invoke(() =>
            {
                mainWindow.NumberOfGuestsLabel.Content = pub.Guests.Count() + 1;
            });
        }

        private void OnPatronEnters(Patron patron)
        {
            UpdateLabels();
            mainWindow.Dispatcher.Invoke(() =>
            {
                mainWindow.GuestAndBouncerLog.Items.Insert(0, patron.Name);
            });
        }

        internal void ExitSimulation()
        {
            mainWindow.OpenClosePub = RunSimulation;
            throw new NotImplementedException();
        }
    }
}
