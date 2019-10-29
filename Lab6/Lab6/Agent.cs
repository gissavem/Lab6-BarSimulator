namespace Lab6
{
    public abstract class Agent
    {
        public Agent(Pub pub, LogHandler logHandler)
        {
            Pub = pub;
            LogHandler = logHandler;
        }
        public Pub Pub { get; private set; }
        public LogHandler LogHandler { get; private set; }
        public abstract void GoHome();
        public abstract void Simulate();
    }    
}
