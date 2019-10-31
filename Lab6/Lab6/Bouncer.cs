using System;
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
        private int minWaitTime = 3000;
        private int maxWaitTime = 10000;
        private bool hasBusArrived = false;
        private DateTime busArriveTime;
        private DateTime pubClosingTime;
        public Bouncer(Pub pub, LogHandler logHandler):base(pub, logHandler)
        {
            CheckForPubSetting();
        }
        private void CheckForPubSetting()
        {
            if (Pub.CurrentSetting == PubSetting.DoubleGuestTime)
            {
                guestDrinkingModifer = 2;
            }
            else if (Pub.CurrentSetting == PubSetting.CouplesNight)
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
            SetBusTimer();
            SetBouncerWorkDuration();
            var welcomeGuestsTask = Task.Run(() =>
            {
                while (ct.IsCancellationRequested == false)
                {                    
                    WelcomeNextPatron();
                }
            });
        }
        private void SetBusTimer()
        {
            if (Pub.CurrentSetting == PubSetting.BusLoad)
            {
                busArriveTime = DateTime.Now + new TimeSpan(0, 0, 20);
            }
        }
        private void SetBouncerWorkDuration()
        {
            if (Pub.CurrentSetting == PubSetting.FiveMinuteBar)
            {
                pubClosingTime = Pub.OpeningTimeStamp + new TimeSpan(0, 5, 0);
            }
            else
            {
                pubClosingTime = Pub.OpeningTimeStamp + new TimeSpan(0, 2, 0);
            }
        }
        private void WelcomeNextPatron()
        {

            if (isWorking == false)
            {
                return;
            }
            WaitForGuests();
            for (int i = 0; i < guestsToLetIn; i++)
            {
                Patron patronToLetIn = LetPatronInside(CheckID);
                Pub.AddGuest(patronToLetIn);
            }
            if (Pub.CurrentState == PubState.PreOpening)
                ChangePubState(PubState.Open);
        }


        private void WaitForGuests()
        {
            int timeToWait = random.Next(minWaitTime, maxWaitTime) * speedModifier;
            DateTime waitTime = DateTime.Now + new TimeSpan(0, 0, 0, 0, timeToWait);
            while (DateTime.Now < waitTime)
            {
                Thread.Sleep(10);
                if (ShouldGoHome())
                {
                    GoHome();
                    break;
                }
                if (ShouldWaitForBus())
                {
                    CheckForBus();
                }
            }
        }

        private bool ShouldGoHome()
        {
            if (DateTime.Now > pubClosingTime)
            {
                return true;
            }
            return false;
        }

        private bool ShouldWaitForBus()
        {
            return Pub.CurrentSetting == PubSetting.BusLoad && hasBusArrived == false;
        }
        private void CheckForBus()
        {
            if (DateTime.Now > busArriveTime)
            {
                LetBusIn();
            }
        }
        public void LetBusIn()
        {
            for (int i = 0; i < 15; i++)
            {
                Patron patronToLetIn = LetPatronInside(CheckID);
                Pub.AddGuest(patronToLetIn);
            }
            hasBusArrived = true;
        }
        private Patron LetPatronInside(Func<string> CheckID)
        {

            var patron = new Patron(Pub.GetGuestCount(), CheckID(), Pub, LogHandler, guestDrinkingModifer);
            LogHandler.UpdateLog($" {patron.Name} joined the party.", LogHandler.MainWindow.GuestAndBouncerLog);
            return patron;
        }
        private string CheckID()
        {
            return NameList.AvailableNames.Take();
        }
        private void ChangePubState(PubState state)
        {
            Pub.ChangeOpenStatus(state);
        }
        public override void GoHome() 
        {
            cts.Cancel();
            isWorking = false;
            LogHandler.UpdateLog("The bouncer went home.", LogHandler.MainWindow.GuestAndBouncerLog);
            ChangePubState(PubState.Closed);
        }
    }
}
