using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    public class Patron : Agent
    {
        private Random random = new Random();
        CancellationTokenSource cts = new CancellationTokenSource();

        public Patron(int indexNumber, string name, Pub pub, LogHandler logHandler) : base(pub, logHandler)
        {
            var ct = cts.Token;
            IndexNumber = indexNumber;
            Name = name;
            var simulateGuest = Task.Run(()=>Simulate());
        }
        public int IndexNumber { get; }
        public string Name { get; set; }
        public Glass Beer { get; set; }
        public bool IsSittingDown { get; set; }
        public bool HasBeenServed { get; set; }

        public override void Simulate()
        {
            Thread.Sleep(1000);
            LogHandler.UpdateLog($" {Name} went to the bar.", LogHandler.MainWindow.GuestAndBouncerLog);
            Pub.BarQueue.Add(this);

            while (HasBeenServed == false)
            {
                Thread.Sleep(10);
            }

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
            }
        
            if (Beer.HasBeer == true && IsSittingDown)
            {
 
                LogHandler.UpdateLog($" {Name} sat down, and is drinking their beer",
                                        LogHandler.MainWindow.GuestAndBouncerLog);
                Thread.Sleep(random.Next(10000, 20000));
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