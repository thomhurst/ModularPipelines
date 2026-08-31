using MEL.Spectre;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Console;

internal sealed class NoopSpectreConsoleLoggerControl(
    ILoggerFactory loggerFactory,
    IOptionsMonitor<LoggerFilterOptions> filterOptions)
    : ISpectreConsoleLoggerControl
{
    public object SynchronizationLock { get; } = new();

    internal bool HasConsoleProvider => loggerFactory.GetType() == typeof(LoggerFactory)
        && LoggerFactoryProviderAccessor.GetCurrentProviders(loggerFactory)
            .Any(static provider => provider is ConsoleLoggerProvider);

    public Task FlushAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public IDisposable Suspend() => NoopDisposable.Instance;

    public bool TryAcquireRenderGate(TimeSpan timeout, out IDisposable? gate)
    {
        gate = null;
        return false;
    }

    public ValueTask<IDisposable?> TryAcquireRenderGateAsync(
        TimeSpan timeout,
        CancellationToken cancellationToken = default) =>
        ValueTask.FromResult<IDisposable?>(null);

    public bool WouldRender(string categoryName, LogLevel logLevel) =>
        HasConsoleProvider
        && LoggerFilterRuleEvaluator.IsEnabled(
            filterOptions.CurrentValue,
            typeof(ConsoleLoggerProvider),
            categoryName,
            logLevel);

    private sealed class NoopDisposable : IDisposable
    {
        public static NoopDisposable Instance { get; } = new();

        public void Dispose()
        {
        }
    }
}
