namespace ModularPipelines.Configuration;

internal sealed record ModuleRetryConfiguration(
    int Count,
    TimeSpan BaseDelay,
    TimeSpan MaxDelay,
    Func<Exception, bool>? ShouldRetry)
{
    internal Func<IModuleContext, Exception, int, CancellationToken, ValueTask<bool>>? ShouldRetryAsync { get; init; }

    internal static TimeSpan DefaultBaseDelay { get; } = TimeSpan.FromMilliseconds(100);
    internal static TimeSpan DefaultMaxDelay { get; } = TimeSpan.FromSeconds(30);
}
