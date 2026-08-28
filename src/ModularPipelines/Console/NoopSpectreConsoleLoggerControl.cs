using MEL.Spectre;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Console;

internal sealed class NoopSpectreConsoleLoggerControl : ISpectreConsoleLoggerControl
{
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

    public bool WouldRender(string categoryName, LogLevel logLevel) => false;

    private sealed class NoopDisposable : IDisposable
    {
        public static NoopDisposable Instance { get; } = new();

        public void Dispose()
        {
        }
    }
}
