using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Console;

internal interface INonSpectreLoggerFactory
{
    IReadOnlyList<ILogger> CreateLoggers(string categoryName);
}

internal sealed class NonSpectreLoggerFactory(
    ILoggerFactory loggerFactory,
    ISpectreLoggerSuppression suppression) : INonSpectreLoggerFactory
{
    private readonly ConcurrentDictionary<string, IReadOnlyList<ILogger>> _loggers = new();

    public IReadOnlyList<ILogger> CreateLoggers(string categoryName)
    {
        return _loggers.GetOrAdd(
            categoryName,
            name => [new SpectreSuppressingLogger(loggerFactory.CreateLogger(name), suppression)]);
    }

    private sealed class SpectreSuppressingLogger(
        ILogger inner,
        ISpectreLoggerSuppression suppression) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
            => inner.BeginScope(state);

        public bool IsEnabled(LogLevel logLevel) => inner.IsEnabled(logLevel);

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            using var scope = suppression.BeginSuppression();
            inner.Log(logLevel, eventId, state, exception, formatter);
        }
    }
}
