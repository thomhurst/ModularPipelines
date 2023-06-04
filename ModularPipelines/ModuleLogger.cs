using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Options;

namespace ModularPipelines;

public class ModuleLogger<T> : ILogger<T>, IDisposable
{
    private readonly IOptions<PipelineOptions> _options;
    private readonly ILogger<T> _defaultLogger;

    private List<(LogLevel logLevel, EventId eventId, object state, Exception? exception,
        Func<object, Exception?, string>? formatter)> _logEvents = new();

    private bool _isDisposed;

    // ReSharper disable once ContextualLoggerProblem
    public ModuleLogger(IOptions<PipelineOptions> options, ILogger<T> defaultLogger)
    {
        _options = options;
        _defaultLogger = defaultLogger;
    }
    
    public IDisposable BeginScope<TState>(TState state)
    {
        return new NoopDisposable();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= _options.Value.LoggerOptions.LogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string>? formatter)
    {
        if (!IsEnabled(logLevel) || _isDisposed)
        {
            return;
        }

        var mappedFormatter = MapFormatter(formatter);
        
        _logEvents.Add((logLevel, eventId, state, exception, mappedFormatter));
    }

    private Func<object, Exception?, string?>? MapFormatter<TState>(Func<TState,Exception?,string>? formatter)
    {
        return (o, exception) => formatter?.Invoke((TState) o, exception);
    }

    private class NoopDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Dispose()
    {
        _isDisposed = true;

        var logEvents = Interlocked.Exchange(ref _logEvents!, new List<(LogLevel logLevel, EventId eventId, object state, Exception exception, Func<object, Exception, string> formatter)>());
        foreach (var (logLevel, eventId, state, exception, formatter) in logEvents)
        {
            _defaultLogger.Log(logLevel, eventId, state, exception, formatter!);
        }
        
        logEvents.Clear();
        _logEvents.Clear();
        
        GC.SuppressFinalize(this);
    }
}