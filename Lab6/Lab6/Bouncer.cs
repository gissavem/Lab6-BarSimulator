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
        private readonly Pub pub;
        private Random random = new Random();
        private CancellationTokenSource cts = new CancellationTokenSource();
        public static event Action<Patron> PatronEnters;
        //ITS ME, BLACKSMITH

        public Bouncer(Pub pub):base(pub)
        {
            this.pub = pub;
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
                    Thread.Sleep(random.Next(3000, 10001));
                    pub.Guests.Add(LetPatronInside(CheckID));
                }
            });
        }

        public Patron LetPatronInside(Func<string> CheckID)
        {
            var patron = new Patron(CheckID(), pub);
            PatronEnters(patron);
            return patron;
            
        }

        private string CheckID()
        {
            return "Lenart bladh";
        }

        protected override void GoHome()
        {
            cts.Cancel();
        }
    }
}
