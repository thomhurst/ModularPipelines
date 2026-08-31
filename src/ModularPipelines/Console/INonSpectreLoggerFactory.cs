using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
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

internal interface IDirectStructuredLogSink;

internal interface IExclusiveStructuredLogSink : ILogger, IDirectStructuredLogSink;

internal interface ISynchronousConsoleLogger : IDirectStructuredLogSink;

internal interface IBufferedConsoleLogger
{
    bool WouldWrite(IBufferedLogEvent logEvent);

    void Write(IBufferedLogEvent logEvent);

    SynchronousConsoleLogEntry Format<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter);
}

internal readonly record struct SynchronousConsoleLogEntry(
    TextWriter? Destination,
    string? Output,
    ExceptionDispatchInfo? Failure)
{
    public bool HasOutput => !string.IsNullOrEmpty(Output);

    public void Write()
    {
        Failure?.Throw();
        Destination?.Write(Output);
    }
}

internal sealed class NonSpectreLoggerFactory(
    ILoggerFactory loggerFactory,
    ISpectreConsoleLoggerControl loggerControl,
    IOptionsMonitor<LoggerFilterOptions> filterOptions) : INonSpectreLoggerFactory
{
    private readonly object _providerLoggersLock = new();
    private readonly Dictionary<string, ProviderLoggerSnapshot> _providerLoggers = [];

    public IReadOnlyList<ILogger> CreateLoggers(string categoryName)
    {
        if (loggerControl is not NoopSpectreConsoleLoggerControl { HasConsoleProvider: true }
            || loggerFactory.GetType() != typeof(LoggerFactory))
        {
            return [new SpectreSuppressingLogger(
                loggerFactory.CreateLogger(categoryName),
                loggerControl)];
        }

        lock (_providerLoggersLock)
        {
            var currentProviders = LoggerFactoryProviderAccessor.GetCurrentProviders(loggerFactory);
            if (_providerLoggers.TryGetValue(categoryName, out var snapshot)
                && ProvidersMatch(snapshot.Providers, currentProviders))
            {
                return snapshot.Loggers;
            }

            var loggers = CreateProviderLoggers(categoryName, currentProviders);
            _providerLoggers[categoryName] = new ProviderLoggerSnapshot(currentProviders, loggers);
            return loggers;
        }
    }

    private IReadOnlyList<ILogger> CreateProviderLoggers(
        string categoryName,
        IReadOnlyList<ILoggerProvider> providers) =>
        providers
            .Select(provider => CreateProviderLogger(provider, categoryName))
            .ToArray();

    private static bool ProvidersMatch(
        IReadOnlyList<ILoggerProvider> left,
        IReadOnlyList<ILoggerProvider> right) =>
        left.Count == right.Count
        && left.Zip(right).All(pair => ReferenceEquals(pair.First, pair.Second));

    private ILogger CreateProviderLogger(ILoggerProvider provider, string categoryName)
    {
        if (provider is ConsoleLoggerProvider consoleProvider)
        {
            return new SynchronousConsoleLogger(
                categoryName,
                consoleProvider,
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
        IOptionsMonitor<LoggerFilterOptions> filterOptions)
        : ILogger, ISynchronousConsoleLogger, IBufferedConsoleLogger
    {
        [ThreadStatic]
        private static StringWriter? _writer;

        private readonly ConditionalWeakTable<IBufferedLogEvent, FormattedEntryHolder> _formattedEntries = new();

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
            Func<TState, Exception?, string> formatter) =>
            Format(logLevel, eventId, state, exception, formatter).Write();

        public bool WouldWrite(IBufferedLogEvent logEvent) =>
            GetOrFormat(logEvent).HasOutput;

        public void Write(IBufferedLogEvent logEvent)
        {
            if (_formattedEntries.TryGetValue(logEvent, out var formattedEntry))
            {
                _formattedEntries.Remove(logEvent);
                formattedEntry.Entry.Write();
                return;
            }

            logEvent.FormatFor(this).Write();
        }

        public SynchronousConsoleLogEntry Format<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return default;
            }

            var currentOptions = GetOptions(provider).CurrentValue;
            var consoleFormatter = GetFormatter(provider, currentOptions);
            var writer = _writer ??= new StringWriter();
            var builder = writer.GetStringBuilder();
            builder.Clear();
            var logEntry = new LogEntry<TState>(
                logLevel,
                categoryName,
                eventId,
                state,
                exception,
                formatter);
            string? output = null;
            ExceptionDispatchInfo? failure = null;
            try
            {
                consoleFormatter.Write(in logEntry, GetScopeProvider(provider), writer);
                output = builder.Length == 0 ? null : builder.ToString();
            }
            catch (Exception exceptionToCapture)
            {
                failure = ExceptionDispatchInfo.Capture(exceptionToCapture);
            }
            finally
            {
                builder.Clear();
                if (builder.Capacity > 1024)
                {
                    builder.Capacity = 1024;
                }
            }

            var destination = logLevel >= currentOptions.LogToStandardErrorThreshold
                ? System.Console.Error
                : System.Console.Out;
            return new SynchronousConsoleLogEntry(destination, output, failure);
        }

        private SynchronousConsoleLogEntry GetOrFormat(IBufferedLogEvent logEvent)
            => _formattedEntries
                .GetValue(logEvent, entry => new FormattedEntryHolder(entry.FormatFor(this)))
                .Entry;

        private sealed record FormattedEntryHolder(SynchronousConsoleLogEntry Entry);

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

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_options")]
        private static extern ref readonly IOptionsMonitor<ConsoleLoggerOptions> GetOptions(
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

    private sealed record ProviderLoggerSnapshot(
        ILoggerProvider[] Providers,
        IReadOnlyList<ILogger> Loggers);
}

internal static class LoggerFactoryProviderAccessor
{
    [DynamicDependency(
        DynamicallyAccessedMemberTypes.NonPublicFields,
        typeof(LoggerFactory))]
    [DynamicDependency(
        DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields,
        "Microsoft.Extensions.Logging.LoggerFactory+ProviderRegistration",
        "Microsoft.Extensions.Logging")]
    public static ILoggerProvider[] GetCurrentProviders(ILoggerFactory loggerFactory)
    {
        var sync = LoggerFactoryFields.Sync.GetValue(loggerFactory)!;
        lock (sync)
        {
            var registrations = (IEnumerable) LoggerFactoryFields.ProviderRegistrations
                .GetValue(loggerFactory)!;
            return registrations.Cast<object>()
                .Select(registration => (ILoggerProvider) LoggerFactoryFields.Provider
                    .GetValue(registration)!)
                .ToArray();
        }
    }

    private static class LoggerFactoryFields
    {
        private const BindingFlags FieldFlags = BindingFlags.Instance
                                                | BindingFlags.Public
                                                | BindingFlags.NonPublic;

        public static readonly FieldInfo ProviderRegistrations =
            GetRequiredField(typeof(LoggerFactory), "_providerRegistrations");

        public static readonly FieldInfo Sync =
            GetRequiredField(typeof(LoggerFactory), "_sync");

        public static readonly FieldInfo Provider = GetRequiredField(
            ProviderRegistrations.FieldType.GetGenericArguments()[0],
            "Provider");

        [UnconditionalSuppressMessage(
            "Trimming",
            "IL2070",
            Justification = "DynamicDependency preserves LoggerFactory and ProviderRegistration fields.")]
        private static FieldInfo GetRequiredField(Type type, string name) =>
            type.GetField(name, FieldFlags)
            ?? throw new MissingFieldException(type.FullName, name);
    }
}

internal sealed class ProviderDeliveryException(
    IReadOnlyList<ILogger> failedLoggers,
    IReadOnlyList<Exception> exceptions)
    : AggregateException("One or more non-console loggers rejected buffered output.", exceptions)
{
    public IReadOnlyList<ILogger> FailedLoggers { get; } = failedLoggers;
}
