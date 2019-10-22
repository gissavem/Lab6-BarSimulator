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
        private int totalNumberOfChairs;
        private int totalNumberOfGlasses;
        public event Action ClosePub;
        public event Action OpenPub;
        public Pub(int totalNumberOfChairs, int totalNumbersOfGlasses, int openingDuraion)
        {
            this.totalNumberOfChairs = totalNumberOfChairs;
            this.totalNumberOfGlasses = totalNumbersOfGlasses;
            OpeningDuraion = openingDuraion;
            Chairs = new BlockingCollection<Chair>();
            Guests = new BlockingCollection<Patron>();
            Agents = new BlockingCollection<Agent>();

        }

        internal void Open()
        {
            OpenPub();
            var keepOpen = Task.Run(() => 
            {
                Thread.Sleep(120000);
                ClosePub();
            });
        }


        public Bar Bar { get; set;}
        public BlockingCollection<Chair> Chairs { get; set; }
        public BlockingCollection<Patron> Guests { get; set; }
        public BlockingCollection<Agent> Agents { get; set; }

        public int OpeningDuraion { get; }

        internal void Initialize()
        {
            Bar = new Bar();
            for (int i = 0; i < totalNumberOfGlasses; i++)
            {
                Bar.AvailableGlasses.Add(new Glass());
            }
            for (int i = 0; i < totalNumberOfChairs; i++)
            {
                Chairs.Add(new Chair());
            }
            Agents.Add(new Bartender(this));
            Agents.Add(new Waitress(this));
            Agents.Add(new Bouncer(this));
        }

    }
}
