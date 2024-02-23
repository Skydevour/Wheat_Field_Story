public class GameDayEvent
{
    public readonly int Day;
    public readonly Enums.Season Season;

    public GameDayEvent(int day, Enums.Season season)
    {
        Day = day;
        Season = season;
    }
}