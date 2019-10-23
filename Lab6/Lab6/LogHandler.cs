using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Lab6
{
    public class LogHandler
    {

        public LogHandler(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
        }

        public MainWindow MainWindow { get; }

        public void UpdateLog(string s, ListBox logBox)
        {
            MainWindow.Dispatcher.Invoke(() =>
            {
                logBox.Items.Insert(0,
                    (MainWindow.GetTimeAsString() + s));
            });
        }
    }
}
