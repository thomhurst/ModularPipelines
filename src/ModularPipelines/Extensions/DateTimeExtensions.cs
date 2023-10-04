namespace ModularPipelines.Extensions;

internal static class DateTimeExtensions
{
    public static TimeOnly ToTimeOnly(this DateTimeOffset dateTime)
    {
        return new TimeOnly(dateTime.Hour, dateTime.Minute, dateTime.Second);
    }
}