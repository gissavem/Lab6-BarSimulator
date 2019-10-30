using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Media;

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
        public BlockingCollection<Chair> Chairs { get; set; }
        public BlockingCollection<Patron> BarQueue { get; set; }
        public ConcurrentDictionary<int, Patron> Guests { get; set; }
        public BlockingCollection<Agent> Employees { get; set; }
        public int TotalNumberOfGuests { get; set; }
        public LogHandler LogHandler { get; private set; }
        public void Open()
        {
            foreach (var employee in Employees)
            {
                employee.Simulate();
            }
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
        internal void StopJukeBox()
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
    }
}
