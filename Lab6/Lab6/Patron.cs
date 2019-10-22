using System;

namespace Lab6
{
    public class Patron : Agent
    {
        

        public Patron(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}