using System.Collections.Concurrent;

namespace Lab6
{
    public class Bar
    {
        public Bar()
        {
            AvailableGlasses = new BlockingCollection<Glass>();
            UsedGlasses = new BlockingCollection<Glass>();
        }
        public BlockingCollection<Glass> AvailableGlasses { get; private set; }
        public BlockingCollection<Glass> UsedGlasses { get; private set; }
    }
}
