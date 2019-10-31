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
        private BlockingCollection<Chair> chairs; 
        private BlockingCollection<Patron> barQueue;
        private ConcurrentDictionary<int, Patron> guests; 
        private BlockingCollection<Agent> employees;

        public Pub(LogHandler logHandler)
        { 
            chairs = new BlockingCollection<Chair>();
            guests = new ConcurrentDictionary<int, Patron>();
            employees = new BlockingCollection<Agent>();
            barQueue = new BlockingCollection<Patron>();
            LogHandler = logHandler;
            CurrentState = PubState.PreOpening;
        }
        public PubState CurrentState { get; private set; } 
        public PubSetting CurrentSetting { get; set; }
        public DateTime OpeningTimeStamp { get; set; } 
        public int OpeningDuration { get; set; }
        public Bar Bar { get; set;}
        public int TotalNumberOfGuests { get; set; }
        public LogHandler LogHandler { get; private set; }
        public int GlobalSpeedModifer { get; set; }

        public void SetEmployees(BlockingCollection<Agent> employees)
        {
            this.employees = employees;
        }
        public void SetChairs(BlockingCollection<Chair> chairs)
        {
            this.chairs = chairs;        
        }
        public void Open()
        {
            foreach (var employee in employees)
            {
                employee.Simulate();
            }
        }
        public bool CheckForLine()
        {
            return barQueue.Any();
        }
        public Patron GetFirstPatronInLine()
        {
            return barQueue.Take();
        }
        public void GetInBarQueue(Patron patron)
        {
            barQueue.Add(patron);
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
            foreach (var chair in chairs)
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

        public void ChangeOpenStatus(PubState state)
        {
            CurrentState = state;
        }

        public void AddGuest(Patron patronToLetIn)
        {
            guests.TryAdd(guests.Count, patronToLetIn);
            TotalNumberOfGuests++;
        }
        public void RemoveGuest(Patron patron)
        {
            guests.TryRemove(patron.IndexNumber, out _);
        }
        public void StopJukeBox()
        {
            SoundPlayer.Stop();
        }
        public int NumberOfEmptyChairs()
        {
            int emptyChairs = 0;
            foreach (Chair chair in chairs)
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
            foreach (var chair in chairs)
            {
                if (chair.Occupant == patron)
                {
                    chair.Occupant = null;
                }
            }
        }
        public int GetGuestCount()
        {
            return guests.Count();
        }
    }
}
