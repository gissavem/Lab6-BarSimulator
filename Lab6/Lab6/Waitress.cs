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
        public Waitress(Pub pub, LogHandler logHandler) : base(pub, logHandler)
        {

        }
        public override void Simulate()
        {
        }
        public BlockingCollection<Glass> WashDishes(BlockingCollection<Glass> dirtyGlasses)
        {
            return null;
        }

        public override void GoHome()
        {
            throw new NotImplementedException();
        }

        //Bricka för glas
    }
}
