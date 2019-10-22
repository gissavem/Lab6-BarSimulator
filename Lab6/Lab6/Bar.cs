using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    public class Bar
    {
        //collection of clean glasses
        //collection of dirty glasses
        public Bar()
        {

        }

        public BlockingCollection<Glass> AvailableGlasses { get; private set; }
        public BlockingCollection<Glass> UsedGlasses { get; private set; }
    }
}
