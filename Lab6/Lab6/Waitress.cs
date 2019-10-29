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
        private CancellationTokenSource cts = new CancellationTokenSource();
        private int speedModifier = 1;
        private int washingTime = 15000;
        private int fetchingTime = 10000;
        public Waitress(Pub pub, LogHandler logHandler) : base(pub, logHandler)
        {
            Tray = new BlockingCollection<Glass>();
            if (pub.CurrentSetting == PubSetting.FastWaitress)
            {
                speedModifier = 2;
            }
        }
        BlockingCollection<Glass> Tray { get; set; }
        public override void Simulate()
        {
            var startTask = Task.Run(() => StartWorking());
        }
        private void StartWorking()
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
        private void FetchGlasses()
        {
            while (Tray.Any() == false)
            {
                if (Pub.Bar.UsedGlasses.Any())
                {
                    LogHandler.UpdateLog(" fetching dirty glasses", LogHandler.MainWindow.WaitressLog);
                    Thread.Sleep(fetchingTime / speedModifier);
                    foreach (var glass in Pub.Bar.UsedGlasses)
                    {
                        var temp = Pub.Bar.UsedGlasses.Take();
                        Tray.Add(temp);
                    }
                }
                Thread.Sleep(10);
            }
        }
        private void WashDishes()
        {
            LogHandler.UpdateLog(" washing dishes", LogHandler.MainWindow.WaitressLog);
            Thread.Sleep(washingTime / speedModifier);
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
        public override void GoHome()
        {
            cts.Cancel();
            LogHandler.UpdateLog(" drank a beer and went home.", LogHandler.MainWindow.WaitressLog);
        }       
    }
}
