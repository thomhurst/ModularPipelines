using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Console;

internal interface INonSpectreLoggerFactory
{
    IReadOnlyList<ILogger> CreateLoggers(string categoryName);
}

internal sealed class NonSpectreLoggerFactory(
    ILoggerFactory loggerFactory,
    ILoggerProviderRegistry providerRegistry,
    IOptionsMonitor<LoggerFilterOptions> filterOptions,
    ISpectreLoggerSuppression suppression) : INonSpectreLoggerFactory, IDisposable
{
    private readonly ConcurrentDictionary<string, IReadOnlyList<ILogger>> _loggers = new();
    private readonly ConcurrentDictionary<ILoggerProvider, LoggerFactory> _providerFactories =
        new(ReferenceEqualityComparer.Instance);

    // A later user registration can replace ILoggerFactory while leaving the core registry registered.
    // In that case only the effective factory knows which providers should receive the event.
    private readonly bool _isEffectiveFactoryTracked =
        ReferenceEquals(loggerFactory, providerRegistry);

    public IReadOnlyList<ILogger> CreateLoggers(string categoryName)
    {
        return _loggers.GetOrAdd(
            categoryName,
            name => _isEffectiveFactoryTracked
                ? [new DynamicProviderLogger(this, name)]
                : [new SpectreSuppressingLogger(loggerFactory.CreateLogger(name), suppression)]);
    }

    public void Dispose()
    {
        foreach (var factory in _providerFactories.Values)
        {
            factory.Dispose();
        }
    }

    private static bool IsSpectreProvider(ILoggerProvider provider) =>
        provider is SuppressibleSpectreLoggerProvider
        || provider.GetType().FullName is SpectreLoggerSuppressionRegistration.SpectreProviderTypeName;

    private IReadOnlyList<ILogger> GetProviderLoggers(string categoryName) =>
    [
        .. providerRegistry.Providers
            .Where(static provider => !IsSpectreProvider(provider))
            .Select(provider => _providerFactories
                .GetOrAdd(
                    provider,
                    value => new LoggerFactory(
                        [new NonOwningLoggerProvider(value)],
                        new ProviderFilterOptionsMonitor(filterOptions, value.GetType())))
                .CreateLogger(categoryName)),
    ];

    private sealed class DynamicProviderLogger(
        NonSpectreLoggerFactory owner,
        string categoryName) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) =>
            owner.GetProviderLoggers(categoryName).Any(logger => logger.IsEnabled(logLevel));

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            List<ILogger>? failedLoggers = null;
            List<Exception>? exceptions = null;
            foreach (var logger in owner.GetProviderLoggers(categoryName))
            {
                try
                {
                    logger.Log(logLevel, eventId, state, exception, formatter);
                }
                catch (Exception deliveryException)
                {
                    (failedLoggers ??= []).Add(logger);
                    (exceptions ??= []).Add(deliveryException);
                }
            }

            if (failedLoggers is not null)
            {
                throw new ProviderDeliveryException(failedLoggers, exceptions!);
            }
        }
    }

    private sealed class SpectreSuppressingLogger(
        ILogger inner,
        ISpectreLoggerSuppression suppression) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => inner.BeginScope(state);

        public bool IsEnabled(LogLevel logLevel) => inner.IsEnabled(logLevel);

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            using var scope = suppression.BeginSuppression();
            try
            {
                inner.Log(logLevel, eventId, state, exception, formatter);
            }
            catch (ProviderDeliveryException)
            {
                throw;
            }
            catch (Exception deliveryException)
            {
                // ILoggerFactory does not expose the provider that failed. Retrying its
                // composite logger would also invoke providers that already succeeded.
                throw new ProviderDeliveryException([], [deliveryException]);
            }
        }
    }
}

internal sealed class ProviderDeliveryException(
    IReadOnlyList<ILogger> failedLoggers,
    IReadOnlyList<Exception> exceptions)
    : AggregateException("One or more non-console loggers rejected buffered output.", exceptions)
{
    public IReadOnlyList<ILogger> FailedLoggers { get; } = failedLoggers;
}
