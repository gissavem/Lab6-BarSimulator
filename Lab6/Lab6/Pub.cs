using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Media;

namespace Lab6
{
    public enum PubState { PreOpening, Open, Closed }
    public class Pub
    {
        public SoundPlayer soundPlayer;

        public Pub(LogHandler logHandler)
        { 
            Chairs = new BlockingCollection<Chair>();
            Guests = new ConcurrentDictionary<int, Patron>();
            Employees = new BlockingCollection<Agent>();
            LogHandler = logHandler;
            CurrentState = PubState.PreOpening;
        }
        public PubState CurrentState { get; set; }
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

        public void StartJukeBox()
        {
            soundPlayer = new SoundPlayer(@"C:\Users\alexander\Documents\GitHub\Lab6-BarSimulator\Lab6\Sound\starwarsmusic.wav");
            soundPlayer.PlayLooping();
        }

    }
}
