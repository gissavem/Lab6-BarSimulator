using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    class Waitress : Agent
    {
        public Waitress(Pub pub) : base(pub)
        {

        }

        public BlockingCollection<Glass> WashDishes(BlockingCollection<Glass> dirtyGlasses)
        {
            return null;
        }

        //Bricka för glas
    }
}
