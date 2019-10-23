﻿using System;
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
        public event Action ClosePub;
        public event Action OpenPub;
        public Pub()
        { 
            Chairs = new BlockingCollection<Chair>();
            Guests = new BlockingCollection<Patron>();
            Agents = new BlockingCollection<Agent>();

        }
        public DateTime OpeningTimeStamp { get; set; }
        public int OpeningDuration { get; set; }
        public Bar Bar { get; set;}
        public BlockingCollection<Chair> Chairs { get; set; }
        public BlockingCollection<Patron> Guests { get; set; }
        public BlockingCollection<Agent> Agents { get; set; }

        internal void Open()
        {
            OpenPub();
            var keepOpen = Task.Run(() => 
            {
                Thread.Sleep(OpeningDuration);
                ClosePub();
            });
        }

    }
}
