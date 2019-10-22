using System;

namespace Lab6
{
    public class Patron : Agent
    {
        

        public Patron(string name, Pub pub) : base(pub)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}