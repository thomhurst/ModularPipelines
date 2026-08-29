using MEL.Spectre;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Console;

internal interface INonSpectreLoggerFactory
{
    IReadOnlyList<ILogger> CreateLoggers(string categoryName);
}

internal sealed class NonSpectreLoggerFactory(
    ILoggerFactory loggerFactory,
    ISpectreConsoleLoggerControl loggerControl,
    IEnumerable<ILoggerProvider> loggerProviders,
    IOptionsMonitor<LoggerFilterOptions> filterOptions) : INonSpectreLoggerFactory, IDisposable
{
    private readonly LoggerFactory? _ownedFactory = CreateNonConsoleFactory(
        loggerFactory,
        loggerControl,
        loggerProviders,
        filterOptions);

    public IReadOnlyList<ILogger> CreateLoggers(string categoryName)
        => [new SpectreSuppressingLogger(
            (_ownedFactory ?? loggerFactory).CreateLogger(categoryName),
            loggerControl)];

    public void Dispose() => _ownedFactory?.Dispose();

    private static LoggerFactory? CreateNonConsoleFactory(
        ILoggerFactory effectiveFactory,
        ISpectreConsoleLoggerControl control,
        IEnumerable<ILoggerProvider> providers,
        IOptionsMonitor<LoggerFilterOptions> options)
    {
        if (control is not NoopSpectreConsoleLoggerControl { HasConsoleProvider: true }
            || effectiveFactory.GetType() != typeof(LoggerFactory))
        {
            return null;
        }

        return new LoggerFactory(
            providers
                .Where(static provider => provider is not ConsoleLoggerProvider)
                .Select(static provider => new NonDisposingLoggerProvider(provider)),
            options);
    }

    private sealed class NonDisposingLoggerProvider(ILoggerProvider inner) : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => inner.CreateLogger(categoryName);

        public void Dispose()
        {
        }
    }

    private sealed class SpectreSuppressingLogger(
        ILogger inner,
        ISpectreConsoleLoggerControl loggerControl) : ILogger
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
            using var scope = loggerControl.Suspend();
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
