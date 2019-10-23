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
            if (Beer == null)
            {
                //wait for beer
                return;
            }
            if (Beer.HasBeer == true)
            {

            }
            else if (Beer.IsDirty == true)
            {

            }
            
        }

        public override void GoHome()
        {
            throw new NotImplementedException();
        }

        public string Name { get; set; }
        public Glass Beer { get; set; }
    }
}