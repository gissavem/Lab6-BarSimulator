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
using System.Windows.Shapes;

namespace Lab6
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly MainWindow mainWindow;

        public SettingsWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            settingsDropBox.ItemsSource = Enum.GetValues(typeof(PubSetting)).Cast<PubSetting>();
            speedDropBox.ItemsSource = Enum.GetValues(typeof(GlobalSpeed)).Cast<GlobalSpeed>();
            confirmSettingsButton.Click += ConfirmSettingsButtonClick;
            this.mainWindow = mainWindow;
        }

        private void ConfirmSettingsButtonClick(object sender, RoutedEventArgs e)
        {
            mainWindow.PubInitializer.Settings = (PubSetting)settingsDropBox.SelectedItem;
            mainWindow.Pub.CurrentSetting = (PubSetting)settingsDropBox.SelectedItem;
            switch ((GlobalSpeed)speedDropBox.SelectedItem)
            {
                case GlobalSpeed.Normal:
                    mainWindow.PubInitializer.GlobalSpeedModifer = 1;
                    mainWindow.Pub.GlobalSpeedModifer = 1;
                    break;
                case GlobalSpeed.TimesTwo:
                    mainWindow.PubInitializer.GlobalSpeedModifer = 2;
                    mainWindow.Pub.GlobalSpeedModifer = 2;
                    break;
                case GlobalSpeed.TimesFour:
                    mainWindow.PubInitializer.GlobalSpeedModifer = 4;
                    mainWindow.Pub.GlobalSpeedModifer = 4;
                    break;
            }
            this.Hide();
            mainWindow.PubInitializer.InitializePub(mainWindow.Pub);
            mainWindow.Show();

        }
    }
}
