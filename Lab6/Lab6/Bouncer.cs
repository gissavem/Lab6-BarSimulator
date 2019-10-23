using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    class Bouncer : Agent
    {
        public static event Action<Patron> PatronEnters;
        public static event Action GoesHome;
        private Random random = new Random();
        private CancellationTokenSource cts = new CancellationTokenSource();
        private bool isWorking = true; 
        //ITS ME, BLACKSMITH

        public Bouncer(Pub pub):base(pub)
        {
            pub.ClosePub += GoHome;
            pub.OpenPub += OnOpenPub;
        }

        private void OnOpenPub()
        {
            var ct = cts.Token;
            var welcomeGuestsTask = Task.Run(() => 
            {
                while (ct.IsCancellationRequested == false)
                {
                    Thread.Sleep(random.Next(3000, 10000));
                    if (isWorking == false)
                    {
                        return;
                    }
                    Pub.Guests.Add(LetPatronInside(CheckID));
                }
            });
        }

        public Patron LetPatronInside(Func<string> CheckID)
        {
            var patron = new Patron(CheckID(), Pub);
            PatronEnters(patron);
            return patron;
            
        }

        private string CheckID()
        {
            return NameList.AvailableNames.Take();
        }

        protected override void GoHome()
        {
            cts.Cancel();
            isWorking = false;
            GoesHome();
        }
    }
}
