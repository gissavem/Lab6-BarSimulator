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
        Action OpenClosePub;
        public MainWindow()
        {
            InitializeComponent();
            OpenCloseButton.Click += OnOpenCloseClick;
            var pubSimulator = new PubSimulator();
            if (pubSimulator.IsSimulating == false)
            {
                OpenClosePub = pubSimulator.RunSimulation;
            }
            else
            {
                OpenClosePub = pubSimulator.ExitSimulation;
            }
            
        }

        private void OnOpenCloseClick(object sender, RoutedEventArgs e)
        {
            OpenClosePub();
            return;
        }
    }
}
