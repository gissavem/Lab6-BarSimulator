using System;
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
            Bouncer.PatronEnters += OnPatronEnters;
            Bouncer.BouncerGoesHome += OnBouncerGoesHome;
            var pubInitializer = new PubInitializer();
            pub = new Pub();
            pub.Bar = pubInitializer.GenerateBar(8);
            pub.Agents = pubInitializer.GenerateEmployees(pub);
            pub.Chairs = pubInitializer.GenereateChairs(9);
            pub.OpeningTimeStamp = pubInitializer.SetOpeningTimestamp();
            pub.OpeningDuration = pubInitializer.SetOpeningDuration();
            var pubSimulator = new PubSimulator(pub, this);
            OpenClosePub = pubSimulator.RunSimulation;
            
        }

        private void OnBouncerGoesHome()
        {
            Dispatcher.Invoke(() =>
            {
                GuestAndBouncerLog.Items.Insert(0,
                    (GetTimeAsString(pub) + 
                    " The bouncer leaves."));
            });
        }

        public void UpdateLabels()
        {
           Dispatcher.Invoke(() =>
            {
                NumberOfGuestsLabel.Content = "Number of guests: " + (pub.Guests.Count() + 1);
            });
        }

        private void OnPatronEnters(Patron patron)
        {
            UpdateLabels();
            Dispatcher.Invoke(() =>
            {
                GuestAndBouncerLog.Items.Insert(0,
                    (GetTimeAsString(pub) + " " + patron.Name +
                    " joins the party."));
            });
        }

        private void OnOpenCloseClick(object sender, RoutedEventArgs e)
        {
            OpenClosePub();

            return;
        }
        private static string GetTimeAsString(Pub pub)
        {
            return (GetTimeSpan(pub.OpeningTimeStamp).Minutes) + ":" + (GetTimeSpan(pub.OpeningTimeStamp).Seconds);
        }
        private static TimeSpan GetTimeSpan(DateTime openingTime)
        { 
            return DateTime.Now - openingTime;
        }

    }
}
