using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Media;
using System.Linq;

namespace Lab6
{
    public class Pub
    {
        public SoundPlayer SoundPlayer;

        public Pub(LogHandler logHandler)
        { 
            Chairs = new BlockingCollection<Chair>();
            Guests = new ConcurrentDictionary<int, Patron>();
            Employees = new BlockingCollection<Agent>();
            BarQueue = new BlockingCollection<Patron>();
            LogHandler = logHandler;
            CurrentState = PubState.PreOpening;
        }
        public PubState CurrentState { get; set; }


        public PubSetting CurrentSetting { get; set; }
        public DateTime OpeningTimeStamp { get; set; }
        public int OpeningDuration { get; set; }
        public Bar Bar { get; set;}
        private BlockingCollection<Chair> Chairs { get; set; }
        private BlockingCollection<Patron> BarQueue { get; set; }
        private ConcurrentDictionary<int, Patron> Guests { get; set; }
        private BlockingCollection<Agent> Employees { get; set; }
        public int TotalNumberOfGuests { get; set; }
        public LogHandler LogHandler { get; private set; }
        public void SetEmployees(BlockingCollection<Agent> employees)
        {
            Employees = employees;
        }
        public void SetChairs(BlockingCollection<Chair> chairs)
        {
            Chairs = chairs;        
        }
        public void Open()
        {
            foreach (var employee in Employees)
            {
                employee.Simulate();
            }
        }
        public bool CheckForLine()
        {
            return BarQueue.Any();
        }
        public Patron GetFirstPatronInLine()
        {
            return BarQueue.Take();
        }
        public void GetInBarQueue(Patron patron)
        {
            BarQueue.Add(patron);
        }
        public void StartJukeBox()
        {
            var playMusic = Task.Run(() => 
            {
                SoundPlayer = new SoundPlayer();
                SoundPlayer.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\Sound\\starwarsmusic.wav";
                SoundPlayer.PlayLooping();
            });
        }

        public bool TryGetChair(out Chair chairToReturn)
        {
            foreach (var chair in Chairs)
            {
                if (chair.Occupant == null)
                {
                    chairToReturn = chair;
                    return true;
                }
            }
            chairToReturn = null;
            return false;
        }
        public void AddGuest(Patron patronToLetIn)
        {
            Guests.TryAdd(Guests.Count, patronToLetIn);
            TotalNumberOfGuests++;
        }
        public void RemoveGuest(Patron patron)
        {
            Guests.TryRemove(patron.IndexNumber, out _);
        }
        public void StopJukeBox()
        {
            SoundPlayer.Stop();
        }
        public int NumberOfEmptyChairs()
        {
            int emptyChairs = 0;
            foreach (Chair chair in Chairs)
            {
                if (chair.Occupant == null)
                {
                    emptyChairs++;
                }
            }
            return emptyChairs;
        }
        public bool CanEmployeesLeave()
        {
            if (CurrentState == PubState.Closed && TotalNumberOfGuests == 0)
            {
                return true;
            }
            return false;
        }
        public void RemovePatronFromChair(Patron patron)
        {
            foreach (var chair in Chairs)
            {
                if (chair.Occupant == patron)
                {
                    chair.Occupant = null;
                }
            }
        }
        public int GetGuestCount()
        {
            return Guests.Count();
        }
    }
}
