using System.Windows.Controls;

namespace Lab6
{
    public class LogHandler
    {
        public LogHandler(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
        }
        public MainWindow MainWindow { get; private set; }
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
