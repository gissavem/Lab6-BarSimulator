using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    class PubInitializer
    {
        
        public PubInitializer(int numberOfGlasses, int numberOfChairs, int openingDuration)
        {
            NumberOfGlasses = numberOfGlasses;
            NumberOfChairs = numberOfChairs;
            OpeningDuration = openingDuration;
        }

        public int NumberOfGlasses { get; set; }
        public int NumberOfChairs { get; set; }
        public int OpeningDuration { get; set; }

        internal void InitializePub(Pub pub)
        {
            pub.Bar = GenerateBar(20);
            pub.Employees = GenerateEmployees(pub, pub.LogHandler);
            pub.Chairs = GenereateChairs(3);
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
        public int SetOpeningDuration()
        {
            int duration = 120000;
            return duration;
        }
    }
}
