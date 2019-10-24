﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Action OpenClosePub;
        private Pub pub;
        public MainWindow()
        {
            InitializeComponent();
            OpenCloseButton.Click += OnOpenCloseClick;

            var logHandler = new LogHandler(this);
            pub = new Pub(logHandler);
            var pubInitializer = new PubInitializer();
            pubInitializer.InitializePub(pub);
            var pubSimulator = new PubSimulator(pub, this);
            OpenClosePub = pubSimulator.RunSimulation;
            
        }

        public string GetTimeAsString()
        {
            return (GetTimeSpan(pub.OpeningTimeStamp).Minutes) + ":" + (GetTimeSpan(pub.OpeningTimeStamp).Seconds);
        }
        public static TimeSpan GetTimeSpan(DateTime openingTime)
        { 
            return DateTime.Now - openingTime;
        }
        public void UpdateLabels()
        {
            Dispatcher.Invoke(() =>
            {
                NumberOfGuestsLabel.Content = "Number of guests: " + (pub.Guests.Count() + 1);
            });
        }
        private void OnOpenCloseClick(object sender, RoutedEventArgs e)
        {
            OpenClosePub();
            pub.StartJukeBox();
            return;
        }
    }
}
