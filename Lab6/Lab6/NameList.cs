using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    public static class NameList
    {
        public static BlockingCollection<string> AvailableNames = new BlockingCollection<string>
        { 
            "Astrid Lindgren", "Lille Skutt", "Jesus Kristus", "Fjodor Dostojevski", "Greta Garbo", "Amy Diamond",
            "John Andersson", "Alexander Bertillson", "Emil Martini", "Daniel Andersson", "Pontus Lindgren",
            "Olof Palme", "Berit Bredstjärt", "Dustin Hoffman", "Sylvester Stalone", "Angela Merkel", "Fredrik Reinfeldt",
            "Ett Fyllesvin", "Lasse Åberg", "Ben Rangel", "Didrik Fetknopp", "Wildkids Ola", "Gusten Grodslukare", 
            "Torsten Flinck", "Sebbe Staxx", "Lisa Lusis", "Lennart Bladh", "Kajsa Warg", "Tupac Shakur", "Augustus Ceasar"
        };

        public static BlockingCollection<string> UsedNames;
    }
}
