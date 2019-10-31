using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    class Bouncer : Agent
    {
        private Random random = new Random();
        private CancellationTokenSource cts = new CancellationTokenSource();
        private bool isWorking = true;
        private Random rnd = new Random();
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

        public override async void Simulate()
        {
            var ct = cts.Token;
            SetBusTimer();
            SetBouncerWorkDuration();

                await Task.Run(() =>
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
                busArriveTime = DateTime.Now + new TimeSpan(0, 0, 20 / Pub.GlobalSpeedModifer);
            }
        }
        private void SetBouncerWorkDuration()
        {
            if (Pub.CurrentSetting == PubSetting.FiveMinuteBar)
            {
                pubClosingTime = Pub.OpeningTimeStamp + new TimeSpan(0,0, 0,0,300000 / Pub.GlobalSpeedModifer);
            }
            else
            {
                pubClosingTime = Pub.OpeningTimeStamp + new TimeSpan(0,0,0,0,120000 / Pub.GlobalSpeedModifer);
            }
        }
        private void WelcomeNextPatron()
        {
            WaitForGuests();

            if (isWorking == false)
            {
                return;
            }
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
            int timeToWait = random.Next(minWaitTime, maxWaitTime) * speedModifier / Pub.GlobalSpeedModifer;
            DateTime waitTime = DateTime.Now + new TimeSpan(0, 0, 0, 0, timeToWait);
            while (DateTime.Now < waitTime)
            {
                if (ShouldGoHome())
                {
                    GoHome();
                    
                    break;
                }
                if (ShouldWaitForBus())
                {
                    CheckForBus();
                }
                Thread.Sleep(10);
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
            int nameIndex = rnd.Next(NameList.AvailableNames.Count());
            string name = NameList.AvailableNames[nameIndex];
            return name;
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
