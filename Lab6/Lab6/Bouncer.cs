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
        private Random random = new Random();
        private CancellationTokenSource cts = new CancellationTokenSource();
        private bool isWorking = true;
        private int guestDrinkingModifer = 1;
        private int guestsToLetIn = 1;
        private int speedModifier = 1;
        //ITS ME, BLACKSMITH

        public Bouncer(Pub pub, LogHandler logHandler):base(pub, logHandler)
        {
            if (Pub.CurrentSetting == PubSetting.DoubleGuestTime)
            {
                guestDrinkingModifer = 2;
            }
            else if(Pub.CurrentSetting == PubSetting.CouplesNight)
            {
                guestsToLetIn = 2;
            }
            else if (Pub.CurrentSetting == PubSetting.BusLoad)
            {
                speedModifier = 2;
            }
        }


        public override void Simulate()
        {
            var ct = cts.Token;
            var welcomeGuestsTask = Task.Run(() =>
            {
                while (ct.IsCancellationRequested == false)
                {
                    CheckNextpatronInLine();
                }
            });
        }

        public Patron LetPatronInside(Func<string> CheckID)
        {

            var patron = new Patron(Pub.Guests.Count, CheckID(), Pub, LogHandler, guestDrinkingModifer);
            LogHandler.UpdateLog($" {patron.Name} joined the party.", LogHandler.MainWindow.GuestAndBouncerLog);
            return patron;
        }

        private string CheckID()
        {
            return NameList.AvailableNames.Take();
        }

        public override void GoHome()
        {
            cts.Cancel();
            isWorking = false;
            LogHandler.UpdateLog("The bouncer went home.", LogHandler.MainWindow.GuestAndBouncerLog);
            Pub.CurrentState = PubState.Closed;
        }

        public void CheckNextpatronInLine()
        {
            Thread.Sleep(random.Next(3000, 10000));
            if (isWorking == false)
            {
                return;
            }
            for (int i = 0; i < guestsToLetIn; i++)
            {
                Pub.Guests.TryAdd(Pub.Guests.Count, LetPatronInside(CheckID));
                Pub.TotalNumberOfGuests++;
            }
            if (Pub.CurrentState == PubState.PreOpening)
                Pub.CurrentState = PubState.Open;
        }

        
    }
}
