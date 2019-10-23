using System;

namespace Lab6
{
    public class Patron : Agent
    {
        

        public Patron(string name, Pub pub, LogHandler logHandler) : base(pub, logHandler)
        {
            Name = name;
        }
        public override void Simulate()
        {
            throw new NotImplementedException();
        }

        public override void GoHome()
        {
            throw new NotImplementedException();
        }

        public string Name { get; set; }
        public Glass Beer { get; set; }
    }
}