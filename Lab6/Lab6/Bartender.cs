using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    class Bartender : Agent
    {
        private CancellationTokenSource cts = new CancellationTokenSource();
        public Bartender(Pub pub, LogHandler logHandler) : base(pub, logHandler) { }
        public override void Simulate()
        {
            var isServingPatrons = Task.Run(()=>ServeGuests());            
        }
        private void ServeGuests()
        {
            var ct = cts.Token;
            while (ct.IsCancellationRequested == false)
            {
                if (Pub.CanEmployeesLeave())
                {
                    GoHome();
                    return;
                }
                ServeNextPatron();
            }
        }
        private void ServeNextPatron()
        {
            Thread.Sleep(10);
            if (Pub.BarQueue.Any())
            {
                ServePatronBeer(Pub.BarQueue.Take());
            }
        }
        private void ServePatronBeer(Patron patron)
        {
            var beerToServe = GetGlass();
            PourBeer(beerToServe, patron);
            patron.Beer = beerToServe;
            patron.HasBeenServed = true;
        }
        private Glass GetGlass()
        {
           while (Pub.Bar.HasAvailableGlasses() == false)
           {
                Thread.Sleep(10);
           }
           Thread.Sleep(3000);
           LogHandler.UpdateLog(" fetched a glass", LogHandler.MainWindow.BartenderLog);
           return Pub.Bar.GetOneCleanGlass();
        }
        private void PourBeer(Glass beerToServe, Patron patron)
        {
            Thread.Sleep(3000);
            LogHandler.UpdateLog($" poured {patron.Name} a beer.", LogHandler.MainWindow.BartenderLog);
            beerToServe.HasBeer = true;
        }

        public override void GoHome()
        {
            cts.Cancel();
            LogHandler.UpdateLog(" closed bar and went home.", LogHandler.MainWindow.BartenderLog);
        }
    }
}
