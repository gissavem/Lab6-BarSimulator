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
            confirmSettingsButton.Click += ConfirmSettingsButtonClick;
            this.mainWindow = mainWindow;
        }

        private void ConfirmSettingsButtonClick(object sender, RoutedEventArgs e)
        {
            mainWindow.PubInitializer.Settings = (PubSetting)settingsDropBox.SelectedItem;
            mainWindow.Pub.CurrentSetting = (PubSetting)settingsDropBox.SelectedItem;
            this.Hide();
            mainWindow.PubInitializer.InitializePub(mainWindow.Pub);
            mainWindow.Show();

        }
    }
}
