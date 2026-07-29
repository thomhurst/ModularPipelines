using Microsoft.Extensions.Logging;

namespace ModularPipelines.Console;

internal sealed class ProviderTrackingLoggerFactory(
    ILoggerFactory inner,
    IEnumerable<ILoggerProvider> providers,
    bool disposeInner = true) : ILoggerFactory, ILoggerProviderRegistry
{
    private readonly Lock _lock = new();
    private readonly List<ILoggerProvider> _providers = [.. providers];
    private int _isDisposed;

    public IReadOnlyList<ILoggerProvider> Providers
    {
        get
        {
            lock (_lock)
            {
                return [.. _providers];
            }
        }
    }

    public void AddProvider(ILoggerProvider provider)
    {
        lock (_lock)
        {
            inner.AddProvider(provider);
            _providers.Add(provider);
        }
    }

    public ILogger CreateLogger(string categoryName) => inner.CreateLogger(categoryName);

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _isDisposed, 1) == 0 && disposeInner)
        {
            inner.Dispose();
        }
    }
}

internal interface ILoggerProviderRegistry
{
    IReadOnlyList<ILoggerProvider> Providers { get; }
}
