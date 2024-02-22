namespace Events.TimeEvents
{
    public class GameMinuteEvent
    {
        public readonly int Second;
        public readonly int Minute;
        public readonly int Hour;

        public GameMinuteEvent(int minute, int hour,int second)
        {
            Second = second;
            Minute = minute;
            Hour = hour;
        }
    }
}
