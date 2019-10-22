using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    public class Pub
    {
        private int totalNumberOfChairs;
        private int totalNumbersOfGlasses;
        public Pub(int totalNumberOfChairs, int totalNumbersOfGlasses, int openingDuraion)
        {
            this.totalNumberOfChairs = totalNumberOfChairs;
            this.totalNumbersOfGlasses = totalNumbersOfGlasses;
            OpeningDuraion = openingDuraion;
        }
        public Bar Bar { get; set;}
        public BlockingCollection<Chair> Chairs { get; set; }
        public BlockingCollection<Patron> Guests { get; set; }
        public BlockingCollection<Agent> Agents { get; set; }

        public int OpeningDuraion { get; }
    }
}
