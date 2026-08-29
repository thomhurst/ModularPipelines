using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using MEL.Spectre;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Console;

internal interface INonSpectreLoggerFactory
{
    IReadOnlyList<ILogger> CreateLoggers(string categoryName);
}

internal interface ISynchronousConsoleLogger;

internal sealed class NonSpectreLoggerFactory(
    ILoggerFactory loggerFactory,
    ISpectreConsoleLoggerControl loggerControl,
    IEnumerable<ILoggerProvider> loggerProviders,
    IOptionsMonitor<LoggerFilterOptions> filterOptions,
    IOptionsMonitor<ConsoleLoggerOptions> consoleOptions) : INonSpectreLoggerFactory
{
    private readonly ILoggerProvider[] _loggerProviders = loggerProviders.ToArray();
    private readonly ConcurrentDictionary<string, IReadOnlyList<ILogger>> _providerLoggers = [];

    public IReadOnlyList<ILogger> CreateLoggers(string categoryName)
    {
        if (loggerControl is not NoopSpectreConsoleLoggerControl { HasConsoleProvider: true }
            || loggerFactory.GetType() != typeof(LoggerFactory))
        {
            return [new SpectreSuppressingLogger(
                loggerFactory.CreateLogger(categoryName),
                loggerControl)];
        }

        return _providerLoggers.GetOrAdd(categoryName, CreateProviderLoggers);
    }

    private IReadOnlyList<ILogger> CreateProviderLoggers(string categoryName) =>
        _loggerProviders
            .Select(provider => CreateProviderLogger(provider, categoryName))
            .ToArray();

    private ILogger CreateProviderLogger(ILoggerProvider provider, string categoryName)
    {
        if (provider is ConsoleLoggerProvider consoleProvider)
        {
            return new SynchronousConsoleLogger(
                categoryName,
                consoleProvider,
                consoleOptions,
                filterOptions);
        }

        var filteredLogger = new FilteredProviderLogger(
            provider.CreateLogger(categoryName),
            provider.GetType(),
            categoryName,
            filterOptions);
        return new SpectreSuppressingLogger(filteredLogger, loggerControl);
    }

    private sealed class FilteredProviderLogger(
        ILogger inner,
        Type providerType,
        string categoryName,
        IOptionsMonitor<LoggerFilterOptions> options) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => inner.BeginScope(state);

        public bool IsEnabled(LogLevel logLevel) =>
            LoggerFilterRuleEvaluator.IsEnabled(
                options.CurrentValue,
                providerType,
                categoryName,
                logLevel)
            && inner.IsEnabled(logLevel);

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                inner.Log(logLevel, eventId, state, exception, formatter);
            }
        }
    }

    private sealed class SynchronousConsoleLogger(
        string categoryName,
        ConsoleLoggerProvider provider,
        IOptionsMonitor<ConsoleLoggerOptions> options,
        IOptionsMonitor<LoggerFilterOptions> filterOptions) : ILogger, ISynchronousConsoleLogger
    {
        [ThreadStatic]
        private static StringWriter? _writer;

        public IDisposable BeginScope<TState>(TState state)
            where TState : notnull => GetScopeProvider(provider).Push(state);

        public bool IsEnabled(LogLevel logLevel) =>
            logLevel != LogLevel.None
            && LoggerFilterRuleEvaluator.IsEnabled(
                filterOptions.CurrentValue,
                provider.GetType(),
                categoryName,
                logLevel);

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var currentOptions = options.CurrentValue;
            var consoleFormatter = GetFormatter(provider, currentOptions);
            var writer = _writer ??= new StringWriter();
            var logEntry = new LogEntry<TState>(
                logLevel,
                categoryName,
                eventId,
                state,
                exception,
                formatter);
            consoleFormatter.Write(in logEntry, GetScopeProvider(provider), writer);

            var builder = writer.GetStringBuilder();
            if (builder.Length == 0)
            {
                return;
            }

            var output = builder.ToString();
            builder.Clear();
            if (builder.Capacity > 1024)
            {
                builder.Capacity = 1024;
            }

            var destination = logLevel >= currentOptions.LogToStandardErrorThreshold
                ? System.Console.Error
                : System.Console.Out;
            destination.Write(output);
        }

        private static ConsoleFormatter GetFormatter(
            ConsoleLoggerProvider provider,
            ConsoleLoggerOptions options)
        {
            var formatters = GetFormatters(provider);
            if (options.FormatterName is not null
                && formatters.TryGetValue(options.FormatterName, out var configuredFormatter))
            {
                return configuredFormatter;
            }

#pragma warning disable CS0618
            var fallbackName = options.Format == ConsoleLoggerFormat.Systemd
                ? ConsoleFormatterNames.Systemd
                : ConsoleFormatterNames.Simple;
#pragma warning restore CS0618
            return formatters[fallbackName];
        }

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_formatters")]
        private static extern ref ConcurrentDictionary<string, ConsoleFormatter> GetFormatters(
            ConsoleLoggerProvider provider);

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_scopeProvider")]
        private static extern ref IExternalScopeProvider GetScopeProvider(
            ConsoleLoggerProvider provider);
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
