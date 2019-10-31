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
        private BlockingCollection<Glass> tray;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private int speedModifier = 1;
        private int washingTime = 15000;
        private int fetchingTime = 10000;
        public Waitress(Pub pub, LogHandler logHandler) : base(pub, logHandler)
        {
            tray = new BlockingCollection<Glass>();
            if (pub.CurrentSetting == PubSetting.FastWaitress)
            {
                speedModifier = 2;
            }
        }
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
            while (TrayIsEmpty())
            {
                if (Pub.Bar.HasUsedGlasses())
                {
                    LogHandler.UpdateLog(" fetching dirty glasses", LogHandler.MainWindow.WaitressLog);
                    Thread.Sleep(fetchingTime / speedModifier / Pub.GlobalSpeedModifer);
                    for (int i = 0; i < Pub.Bar.NumberOfUsedGlasses; i++)
                    {
                        tray.Add(Pub.Bar.GetOneUsedGlass());
                    }
                }
                Thread.Sleep(10);
            }
        }

        private bool TrayIsEmpty()
        {
            return tray.Any() == false;
        }

        private void WashDishes()
        {
            LogHandler.UpdateLog(" washing dishes", LogHandler.MainWindow.WaitressLog);
            Thread.Sleep(washingTime / speedModifier / Pub.GlobalSpeedModifer);
        }
        private void ReturnCleanGlasses()
        {
            Thread.Sleep(3000);
            LogHandler.UpdateLog(" returned glasses to bar", LogHandler.MainWindow.WaitressLog);
            foreach (var glass in tray)
            {

                Pub.Bar.AddAvailableGlass(GlassFromTray());
            }
        }

        private Glass GlassFromTray()
        {
            return tray.Take();
        }

        public override void GoHome()
        {
            cts.Cancel();
            LogHandler.UpdateLog(" drank a beer and went home.", LogHandler.MainWindow.WaitressLog);
        }       
    }
}
