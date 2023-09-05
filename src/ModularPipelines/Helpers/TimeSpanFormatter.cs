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
        return timeSpan.TotalMilliseconds + "ms";
    }
    
    private static string Seconds(TimeSpan timeSpan)
    {
        return timeSpan.TotalSeconds + "s";
    }
    
    private static string Minutes(TimeSpan timeSpan)
    {
        return timeSpan.TotalMinutes + "m";
    }
    
    private static string Hours(TimeSpan timeSpan)
    {
        return timeSpan.TotalHours + "h";
    }
}