namespace ModularPipelines.Helpers;

internal static class TimeSpanFormatter
{
    public static string ToDisplayString(this TimeSpan timeSpan)
    {
        if (timeSpan.TotalSeconds < 1)
        {
            return Milliseconds(timeSpan);
        }

        if (timeSpan.TotalMinutes < 1)
        {
            return $"{Seconds(timeSpan)} & {Milliseconds(timeSpan)}";
        }

        if (timeSpan.TotalHours < 1)
        {
            return $"{Minutes(timeSpan)} & {Seconds(timeSpan)}";
        }

        return $"{Hours(timeSpan)} & {Minutes(timeSpan)}";
    }

    private static string Milliseconds(TimeSpan timeSpan)
    {
        return timeSpan.Milliseconds.ToString("0") + "ms";
    }

    private static string Seconds(TimeSpan timeSpan)
    {
        return timeSpan.Seconds.ToString("0") + "s";
    }

    private static string Minutes(TimeSpan timeSpan)
    {
        return timeSpan.Minutes.ToString("0") + "m";
    }

    private static string Hours(TimeSpan timeSpan)
    {
        return timeSpan.Hours.ToString("0") + "h";
    }
}