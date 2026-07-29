namespace ModularPipelines.Configuration;

internal sealed record ModuleRetryConfiguration(
    int Count,
    TimeSpan BaseDelay,
    Func<Exception, bool>? ShouldRetry)
{
    internal static TimeSpan DefaultBaseDelay { get; } = TimeSpan.FromMilliseconds(100);
}
