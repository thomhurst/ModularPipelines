using MEL.Spectre;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Console;

internal sealed class NoopSpectreConsoleLoggerControl(
    ILoggerFactory loggerFactory,
    IEnumerable<ILoggerProvider> loggerProviders)
    : ISpectreConsoleLoggerControl
{
    private readonly bool _hasConsoleProvider = loggerProviders.Any(static provider =>
        provider is Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider);

    public object SynchronizationLock { get; } = new();

    public Task FlushAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public IDisposable Suspend() => NoopDisposable.Instance;

    public bool TryAcquireRenderGate(TimeSpan timeout, out IDisposable? gate)
    {
        gate = NoopDisposable.Instance;
        return true;
    }

    public ValueTask<IDisposable?> TryAcquireRenderGateAsync(
        TimeSpan timeout,
        CancellationToken cancellationToken = default) =>
        ValueTask.FromResult<IDisposable?>(NoopDisposable.Instance);

    public bool WouldRender(string categoryName, LogLevel logLevel) =>
        _hasConsoleProvider && loggerFactory.CreateLogger(categoryName).IsEnabled(logLevel);

    private sealed class NoopDisposable : IDisposable
    {
        public static NoopDisposable Instance { get; } = new();

        public void Dispose()
        {
        }
    }
}
