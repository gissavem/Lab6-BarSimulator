using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    class Waitress : Agent
    {
        CancellationTokenSource cts = new CancellationTokenSource();

        public Waitress(Pub pub, LogHandler logHandler) : base(pub, logHandler)
        {
            Tray = new BlockingCollection<Glass>();
        }

        BlockingCollection<Glass> Tray { get; set; }
        public override void Simulate()
        {
            var startTask = Task.Run(() => StartTasks());
        }

        private void StartTasks()
        {
            var ct = cts.Token;
            while(ct.IsCancellationRequested == false)
            {
                if (Pub.CanEmployeesLeave())
                {
                    GoHome();
                    return;
                }
                FetchGlasses();
                WashDishes();
                ReturnCleanGlasses();
            }
        }

        private void ReturnCleanGlasses()
        {
            Thread.Sleep(3000);
            LogHandler.UpdateLog(" returned glasses to bar", LogHandler.MainWindow.WaitressLog);
            foreach (var glass in Tray)
            {
                var temp = Tray.Take();
                Pub.Bar.AvailableGlasses.Add(temp);
            }
        }

        private void WashDishes()
        {
            LogHandler.UpdateLog(" washing dishes", LogHandler.MainWindow.WaitressLog);
            Thread.Sleep(15000);
        }

        private void FetchGlasses()
        {
            while (Tray.Any() == false)
            {
                if (Pub.Bar.UsedGlasses.Any())
                {
                    LogHandler.UpdateLog(" fetching dirty glasses", LogHandler.MainWindow.WaitressLog);
                    Thread.Sleep(10000);
                    foreach (var glass in Pub.Bar.UsedGlasses)
                    {
                        var temp = Pub.Bar.UsedGlasses.Take();
                        Tray.Add(temp);
                    }
                }
                Thread.Sleep(10);
            }
        }

        public BlockingCollection<Glass> WashDishes(BlockingCollection<Glass> dirtyGlasses)
        {
            return null;
        }

        public override void GoHome()
        {
            cts.Cancel();
            LogHandler.UpdateLog(" drank a beer and went home.", LogHandler.MainWindow.WaitressLog);
        }

       
    }
}
