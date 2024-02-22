namespace Events.TimeEvents
{
    public class GameHourEvent
    {
        public readonly int Hour;
        public readonly int Day;
        public readonly int Month;
        public readonly int Year;
        public readonly Enums.Season GameSeason;

        public GameHourEvent(int hour, int day, int month, int year, Enums.Season gameSeason)
        {
            Hour = hour;
            Day = day;
            Month = month;
            Year = year;
            GameSeason = gameSeason;
        }
    }
}
