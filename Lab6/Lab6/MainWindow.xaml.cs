using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
        public Timer LabelTimer = new Timer(5);
        public PubInitializer PubInitializer;
        public Pub Pub;
        public MainWindow() // se över vad som skall loggas, kolla spec. tror det saknas bartenderväntar på kund, patron letar efter stol
        {
            InitializeComponent();
            OpenCloseButton.Click += OnOpenCloseClick;
            LabelTimer.Elapsed += OnTimerTick;
            var settingsWindow = new SettingsWindow(this);
            var logHandler = new LogHandler(this);
            Pub = new Pub(logHandler);
            PubInitializer = new PubInitializer();
            var pubSimulator = new PubSimulator(Pub, this);
            OpenClosePub = pubSimulator.RunSimulation;
            settingsWindow.Show();            
            this.Hide();               
        }
        private void OnTimerTick(object sender, ElapsedEventArgs e)
        {
            UpdateLabels();
        }
        private void OnOpenCloseClick(object sender, RoutedEventArgs e)
        {
            OpenClosePub();
            Pub.StartJukeBox();
            return;
        }
        public void UpdateLabels()
        {
            Dispatcher.Invoke(() =>
            {
                NumberOfGuestsLabel.Content = "Number of guests: " + (Pub.TotalNumberOfGuests);
                NumberOfGlassesLabel.Content = "Number of available glasses: " + (Pub.Bar.NumberOfAvailableGlasses);
                EmptyChairsLabel.Content = "Number of empty chairs: " + Pub.NumberOfEmptyChairs();
                Timer.Content = "Time elapsed: " + GetTimeAsString();
            });
        }
        public string GetTimeAsString()
        {
            return (GetTimeSpan(Pub.OpeningTimeStamp).Minutes) + ":" + (GetTimeSpan(Pub.OpeningTimeStamp).Seconds);
        }
        public static TimeSpan GetTimeSpan(DateTime openingTime)
        {
            return DateTime.Now - openingTime;
        }
    }
}
