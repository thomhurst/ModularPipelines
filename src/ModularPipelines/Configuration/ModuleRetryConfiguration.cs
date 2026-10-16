namespace ModularPipelines.Configuration;

internal sealed record ModuleRetryConfiguration(
    int Count,
    TimeSpan BaseDelay,
    TimeSpan MaxDelay,
    Func<Exception, bool>? ShouldRetry)
{
    internal static TimeSpan DefaultBaseDelay { get; } = TimeSpan.FromMilliseconds(100);
    internal static TimeSpan DefaultMaxDelay { get; } = TimeSpan.FromSeconds(30);
}
