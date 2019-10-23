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
            var simulateGuests = Task.Run(Simulate);
        }
        public override void Simulate()
        {
            LogHandler.UpdateLog($" {Name} goes to bar.", LogHandler.MainWindow.GuestAndBouncerLog);

            while (Beer == null)
            {
        
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
            Patron patronToRemove;
            LogHandler.UpdateLog($" {Name} went home.",
                                      LogHandler.MainWindow.GuestAndBouncerLog);
            Pub.Guests.TryRemove(IndexNumber, out patronToRemove);
        }

        public int IndexNumber { get; }
        public string Name { get; set; }
        public Glass Beer { get; set; }
        public bool IsSittingDown { get; set; }
    }
}