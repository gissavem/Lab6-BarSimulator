using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    public class Patron : Agent
    {
        private Random random = new Random();
        private int timeToWalkToBar = 1000;
        private int drinkingTime;
        public Patron(int indexNumber, string name, Pub pub, LogHandler logHandler, int speedModifier) : base(pub, logHandler)
        {
            IndexNumber = indexNumber;
            Name = name;
            drinkingTime = random.Next(20000, 30000) * speedModifier / Pub.GlobalSpeedModifer;
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
            Thread.Sleep(timeToWalkToBar / Pub.GlobalSpeedModifer);
            LogHandler.UpdateLog($" {Name} went to the bar.", LogHandler.MainWindow.GuestAndBouncerLog);
            Pub.GetInBarQueue(this);
        }
        private void WaitForBeer()
        {
            while (InBarQueue())
            {
                Thread.Sleep(10);
            }
        }
        private bool InBarQueue()
        {
            return HasBeenServed == false;
        }
        private void WaitForEmptyChair()
        {
            while (IsLookingForChair())
            {
                Chair chair;
                if (Pub.TryGetChair(out chair))
                {
                    chair.Occupant = this;
                    IsSittingDown = true;
                    break;
                }
                Thread.Sleep(10);
            }
        }
        private bool IsLookingForChair()
        {
            return IsSittingDown == false && Beer != null;
        }
        private void DrinkBeer()
        {
            LogHandler.UpdateLog($" {Name} sat down, and is drinking their beer",
                                    LogHandler.MainWindow.GuestAndBouncerLog);
            Thread.Sleep(drinkingTime / Pub.GlobalSpeedModifer);
            LogHandler.UpdateLog($" {Name} finished their beer.",
                                    LogHandler.MainWindow.GuestAndBouncerLog);
            Pub.Bar.AddUsedGlass(Beer);
            Beer = null;
            GoHome();
        }
        public override void GoHome()
        {
            LogHandler.UpdateLog($" {Name} went home.",
                                      LogHandler.MainWindow.GuestAndBouncerLog);
            Pub.RemoveGuest(this);
            Pub.TotalNumberOfGuests--;
            Pub.RemovePatronFromChair(this);
        }
    }
}