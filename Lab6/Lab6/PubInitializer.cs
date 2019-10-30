using System.Collections.Concurrent;

namespace Lab6
{
    public class PubInitializer
    {
        
        public PubInitializer()
        {
            Settings = PubSetting.Default;
            NumberOfChairs = 9;
            NumberOfGlasses = 8;
            OpeningDuration = 120000;
        }
        public PubSetting Settings { get; set; }
        private int NumberOfGlasses { get; set; }
        private int NumberOfChairs { get; set; }
        private int OpeningDuration { get; set; }
        public void InitializePub(Pub pub)
        {
            GetSettings();
            pub.Bar = GenerateBar(NumberOfGlasses);
            pub.Employees = GenerateEmployees(pub, pub.LogHandler);
            pub.Chairs = GenereateChairs(NumberOfChairs);
            pub.OpeningDuration = OpeningDuration;
        }
        private void GetSettings()
        {
            switch (Settings)
            {
                case PubSetting.TwentyGlasses:
                    NumberOfChairs = 3;
                    NumberOfGlasses = 20;
                    break;
                case PubSetting.TwentyChairs:
                    NumberOfChairs = 20;
                    NumberOfGlasses = 5;
                    break;
                case PubSetting.FiveMinuteBar:
                    OpeningDuration = 300000;
                    break;
            }
        }
        private Bar GenerateBar(int totalNumberOfGlasses)
        {
            var bar = new Bar();
            for (int i = 0; i < totalNumberOfGlasses; i++)
            {
                bar.AddAvailableGlass(new Glass());
            }

            return bar;
        }
        private BlockingCollection<Agent> GenerateEmployees(Pub pub, LogHandler logHandler)
        {
            var employees = new BlockingCollection<Agent>
            {
                new Waitress(pub, logHandler),
                new Bartender(pub, logHandler),
                new Bouncer(pub, logHandler)
            };
            return employees;
        }
        private BlockingCollection<Chair> GenereateChairs(int totalNumberOfChairs)
        {

            var chairs = new BlockingCollection<Chair>();
            for (int i = 0; i < totalNumberOfChairs; i++)
            {
                chairs.Add(new Chair());
            }
            return chairs;
        }
    }
}
