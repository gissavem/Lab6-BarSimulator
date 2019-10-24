﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    class PubInitializer
    {

        public PubInitializer()
        {

        }
        internal void InitializePub(Pub pub)
        {
            pub.Bar = GenerateBar(8);
            pub.Employees = GenerateEmployees(pub, pub.LogHandler);
            pub.Chairs = GenereateChairs(9);
            pub.OpeningTimeStamp = SetOpeningTimestamp();
            pub.OpeningDuration = SetOpeningDuration();
        }

        internal BlockingCollection<Chair> GenereateChairs(int totalNumberOfChairs)
        {

            var chairs = new BlockingCollection<Chair>();
            for (int i = 0; i < totalNumberOfChairs; i++)
            {
                chairs.Add(new Chair());
            }
            return chairs;
        }

        public Bar GenerateBar(int totalNumberOfGlasses)
        {
            var bar = new Bar();
            for (int i = 0; i < totalNumberOfGlasses; i++)
            {
                bar.AvailableGlasses.Add(new Glass());
            }

            return bar;
        }

        public BlockingCollection<Agent> GenerateEmployees(Pub pub, LogHandler logHandler)
        {
            var employees = new BlockingCollection<Agent>
            {
                new Waitress(pub, logHandler),
                new Bartender(pub, logHandler),
                new Bouncer(pub, logHandler)
            };
            return employees;
        }


        public DateTime SetOpeningTimestamp()
        {
            return DateTime.UtcNow;
        }
        public int SetOpeningDuration()
        {
            int duration = 20000;
            return duration;
        }
    }
}
