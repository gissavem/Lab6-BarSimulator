using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    class Bartender : Agent
    {
        private CancellationTokenSource cts = new CancellationTokenSource();
        public Bartender(Pub pub, LogHandler logHandler) :base(pub, logHandler)
        {
        }
        public override void Simulate()
        {
            var ct = cts.Token;
            var isServingPatrons = Task.Run(() =>
            {
                while (ct.IsCancellationRequested == false)
                {
                    foreach (var keyValuePair in Pub.Guests)
                    {
                        if (keyValuePair.Value.Beer == null)
                        {
                            ServePatronBeer(keyValuePair.Value, GetGlass);
                        }
                    }
                }
            });
        }

        private void ServePatronBeer(Patron patron, Func<Glass> getGlass)
        {
            var beerToServe = getGlass();
            PourBeer(beerToServe, patron);
            patron.Beer = beerToServe;

        }

        private void PourBeer(Glass beerToServe, Patron patron)
        {
            LogHandler.UpdateLog($" Pours {patron.Name} a beer.", LogHandler.MainWindow.BartenderLog);
            Thread.Sleep(3000);
            beerToServe.HasBeer = true;
        }

        private Glass GetGlass()
        {
           while (Pub.Bar.AvailableGlasses.Any() == false)
           {
        
           }
           LogHandler.UpdateLog(" gets glass", LogHandler.MainWindow.BartenderLog);
           Thread.Sleep(3000);
           return Pub.Bar.AvailableGlasses.Take();
        }

        public override void GoHome()
        {
            throw new NotImplementedException();
        }
    }
}
