using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6
{
    public class Pub
    {
        public Pub(LogHandler logHandler)
        { 
            Chairs = new BlockingCollection<Chair>();
            Guests = new ConcurrentDictionary<int, Patron>();
            Employees = new BlockingCollection<Agent>();
            LogHandler = logHandler;
        }
        public DateTime OpeningTimeStamp { get; set; }
        public int OpeningDuration { get; set; }
        public Bar Bar { get; set;}
        public BlockingCollection<Chair> Chairs { get; set; }
        public ConcurrentDictionary<int, Patron> Guests { get; set; }
        public BlockingCollection<Agent> Employees { get; set; }
        public LogHandler LogHandler { get; }

        internal void Open()
        {
            foreach (var employee in Employees)
            {
                employee.Simulate();
            }
            var keepOpen = Task.Run(() => 
            {
                Thread.Sleep(OpeningDuration);
                foreach (Agent bouncer in Employees)
                {
                    if (bouncer is Bouncer)
                    {
                        bouncer.GoHome();
                        break;
                    }
                }
            });
            
        }

    }
}
