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
        public static event Action GetsGlass;
        public static event Action<Patron> PoursBeer;
        public static event Action GoesHome;


        public Bartender(Pub pub):base(pub)
        {
            pub.OpenPub += OnOpenPub;
        }

        private void OnOpenPub()
        {
            var ct = cts.Token;
            var isServingPatrons = Task.Run(() =>
            {
                while (ct.IsCancellationRequested == false)
                {
                    foreach (var patron in Pub.Guests)
                    {
                        if (patron.Beer == null)
                        {                            
                            ServePatronBeer(patron, GetGlass);
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
            PoursBeer(patron);
            Thread.Sleep(3000);
            beerToServe.HasBeer = true;
            beerToServe.IsDirty = true;
        }

        private Glass GetGlass()
        {
           while (Pub.Bar.AvailableGlasses.Any() == false)
           {
        
           }
           GetsGlass();
           Thread.Sleep(3000);
           return Pub.Bar.AvailableGlasses.Take();
        }


    }
}
