using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    public class Patron : Agent
    {
        private Random random = new Random();
        private CancellationTokenSource cts = new CancellationTokenSource();
        private int drinkingTime;
        public Patron(int indexNumber, string name, Pub pub, LogHandler logHandler, int speedModifier) : base(pub, logHandler)
        {
            IndexNumber = indexNumber;
            Name = name;
            drinkingTime = random.Next(20000, 30000) * speedModifier;
            var simulateGuest = Task.Run(()=>Simulate());
        }
        public int IndexNumber { get; private set; }
        public string Name { get; private set; }
        public Glass Beer { get; set; }
        public bool IsSittingDown { get; set; }
        public bool HasBeenServed { get; set; }

        public override void Simulate()
        {
            GoToBar();
            WaitForBeer();
            WaitForEmptyChair();
            DrinkBeer();
        }
        private void GoToBar()
        {
            Thread.Sleep(1000);
            LogHandler.UpdateLog($" {Name} went to the bar.", LogHandler.MainWindow.GuestAndBouncerLog);
            Pub.BarQueue.Add(this);
        }

        private void WaitForBeer()
        {
            while (HasBeenServed == false)
            {
                Thread.Sleep(10);
            }
        }
        private void WaitForEmptyChair()
        {
            while (IsSittingDown == false && Beer != null)
            {
                foreach (var chair in Pub.Chairs)
                {
                    if (chair.Occupant == null)
                    {
                        chair.Occupant = this;
                        IsSittingDown = true;
                        break;
                    }
                }
                Thread.Sleep(10);
            }
        }
        private void DrinkBeer()
        {
            if (Beer.HasBeer == true && IsSittingDown)
            {
                LogHandler.UpdateLog($" {Name} sat down, and is drinking their beer",
                                        LogHandler.MainWindow.GuestAndBouncerLog);
                Thread.Sleep(drinkingTime);
                LogHandler.UpdateLog($" {Name} finished their beer.",
                                        LogHandler.MainWindow.GuestAndBouncerLog);
                Pub.Bar.UsedGlasses.Add(Beer);
                Beer = null;
                GoHome();
            }
        }
        public override void GoHome()
        {
            LogHandler.UpdateLog($" {Name} went home.",
                                      LogHandler.MainWindow.GuestAndBouncerLog);
            Pub.Guests.TryRemove(IndexNumber, out _);
            Pub.TotalNumberOfGuests--;
            foreach (var chair in Pub.Chairs)
            {
                if (chair.Occupant == this)
                {
                    chair.Occupant = null;
                }
            }
        }
    }
}