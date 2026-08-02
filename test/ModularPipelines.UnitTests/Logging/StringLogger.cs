using System.Text;
using Microsoft.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace ModularPipelines.UnitTests.Logging;

public class StringLogger<T> : ILogger<T>
{
    private readonly StringBuilder _stringBuilder;
    private readonly LogLevel _minimumLevel;

    public StringLogger(StringBuilder stringBuilder, LogLevel minimumLevel = LogLevel.Trace)
    {
        _stringBuilder = stringBuilder;
        _minimumLevel = minimumLevel;
    }

    /// <inheritdoc/>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var log = formatter.Invoke(state, exception);
        _stringBuilder.AppendLine(log);
    }

    /// <inheritdoc/>
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= _minimumLevel && logLevel != LogLevel.None;
    }

    /// <inheritdoc/>
    public IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        return new NoOpDisposable();
    }

    private class NoOpDisposable : IDisposable
    {
        public void Dispose()
        {
            // Stub class
        }
    }
}
