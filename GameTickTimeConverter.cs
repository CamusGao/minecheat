namespace Minecheat;

public static class GameTickTimeConverter
{
    public static readonly int TICKS_PER_SECOND = 20;

    public static TimeSpan ConvertTicksToTimeSpan(int ticks)
    {
        var totalSeconds = ticks / TICKS_PER_SECOND;
        return TimeSpan.FromSeconds(totalSeconds);
    }

    public static int ConvertTimeSpanToTicks(TimeSpan timeSpan)
    {
        return (int)(timeSpan.TotalSeconds * TICKS_PER_SECOND);
    }
}
