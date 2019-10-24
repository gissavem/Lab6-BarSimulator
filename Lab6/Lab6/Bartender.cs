﻿using System;
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
            var isServingPatrons = Task.Run(()=>ServeGuests());            
        }

        private void ServeGuests()
        {
            var ct = cts.Token;
            while (ct.IsCancellationRequested == false)
            {
                if (Pub.Guests.IsEmpty && Pub.CurrentState == PubState.Open)
                {
                    GoHome();
                    return;
                }

                foreach (var keyValuePair in Pub.Guests)
                {
                    if (keyValuePair.Value.Beer != null || keyValuePair.Value.HasBeenServed == true)
                        continue;

                    ServePatronBeer(keyValuePair.Value);                    
                }
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
           while (Pub.Bar.AvailableGlasses.Any() == false)
           {
        
           }
           Thread.Sleep(3000);
           LogHandler.UpdateLog(" fetched a glass", LogHandler.MainWindow.BartenderLog);
           return Pub.Bar.AvailableGlasses.Take();
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
            Pub.CurrentState = PubState.Closed;
        }
    }
}
