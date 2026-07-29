using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Console;

internal interface ISpectreLoggerFilter
{
    bool IsEnabled(string categoryName, LogLevel logLevel);
}

internal sealed class SpectreLoggerFilter : ISpectreLoggerFilter, IDisposable
{
    private readonly ConcurrentDictionary<string, ILogger> _loggers = new();
    private readonly LoggerFactory _loggerFactory;
    private readonly bool _isEffectiveFactoryTracked;

    public SpectreLoggerFilter(
        SuppressibleSpectreLoggerProvider provider,
        IOptionsMonitor<LoggerFilterOptions> filterOptions,
        ILoggerFactory loggerFactory,
        ILoggerProviderRegistry providerRegistry)
    {
        _isEffectiveFactoryTracked = ReferenceEquals(loggerFactory, providerRegistry);
        _loggerFactory = new LoggerFactory(
            [new NonOwningLoggerProvider(provider)],
            new ProviderFilterOptionsMonitor(filterOptions, provider.GetType()));
    }

    public bool IsEnabled(string categoryName, LogLevel logLevel) =>
        _isEffectiveFactoryTracked
        && _loggers.GetOrAdd(categoryName, _loggerFactory.CreateLogger).IsEnabled(logLevel);

    public void Dispose() => _loggerFactory.Dispose();
}
