using System.Collections.Concurrent;
using System.Linq;

namespace Lab6
{
    public class Bar
    {
        private BlockingCollection<Glass> usedGlasses;
        private BlockingCollection<Glass> availableGlasses;

        public Bar()
        {
            availableGlasses = new BlockingCollection<Glass>();
            usedGlasses = new BlockingCollection<Glass>();
        }
        public int NumberOfUsedGlasses { get { return usedGlasses.Count(); } }
        public int NumberOfAvailableGlasses { get { return availableGlasses.Count(); } }

        public bool HasUsedGlasses()
        {
            return usedGlasses.Any();
        }


        public Glass GetOneUsedGlass()
        {
           return usedGlasses.Take();
        }

        public void AddUsedGlass(Glass glass)
        {
            usedGlasses.Add(glass);
        }

        public bool HasAvailableGlasses()
        {
            return availableGlasses.Any();
        }

        public Glass GetOneCleanGlass()
        {
            return availableGlasses.Take();
        }

        public void AddAvailableGlass(Glass glass)
        {
            availableGlasses.Add(glass);
        }
    }
}
