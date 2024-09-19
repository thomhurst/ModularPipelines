using System.Text;
using Microsoft.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace ModularPipelines.UnitTests;

public class StringLogger<T> : ILogger<T>
{
    private readonly StringBuilder _stringBuilder;

    public StringLogger(StringBuilder stringBuilder)
    {
        _stringBuilder = stringBuilder;
    }

    /// <inheritdoc/>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var log = formatter.Invoke(state, exception);
        _stringBuilder.AppendLine(log);
    }

    /// <inheritdoc/>
    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
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