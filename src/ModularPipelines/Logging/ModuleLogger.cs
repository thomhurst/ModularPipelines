using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;

namespace ModularPipelines.Logging;

internal abstract class ModuleLogger : IModuleLogger
{
    internal static readonly AsyncLocal<IModuleLogger?> Values = new();

    internal static ILogger Current => (Values.Value as ILogger) ?? NullLogger.Instance;

    protected static readonly object DisposeLock = new();
    protected static readonly object LogLock = new();
    protected Exception? _exception;

    internal DateTime LastLogWritten { get; set; } = DateTime.MinValue;

    public abstract void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter);

    public abstract bool IsEnabled(LogLevel logLevel);

    public abstract IDisposable? BeginScope<TState>(TState state)
        where TState : notnull;

    public abstract void Dispose();

    public abstract void LogToConsole(string value);

    public void SetException(Exception exception)
    {
        _exception = exception;
    }
}

internal class ModuleLogger<T> : ModuleLogger, IModuleLogger, ILogger<T>
{
    private readonly ILogger<T> _defaultLogger;
    private readonly ISecretObfuscator _secretObfuscator;
    private readonly IConsoleWriter _consoleWriter;
    private readonly ISmartCollapsableLoggingStringBlockProvider _collapsableLoggingStringBlockProvider;
    private readonly IExceptionBuffer _exceptionBuffer;
    private readonly IFormattedLogValuesObfuscator _formattedLogValuesObfuscator;
    private readonly ILogEventBuffer _logEventBuffer;

    private bool _isDisposed;

    // ReSharper disable once ContextualLoggerProblem
    public ModuleLogger(ILogger<T> defaultLogger,
        IModuleLoggerContainer moduleLoggerContainer,
        ISecretObfuscator secretObfuscator,
        IConsoleWriter consoleWriter,
        ISmartCollapsableLoggingStringBlockProvider collapsableLoggingStringBlockProvider,
        IExceptionBuffer exceptionBuffer,
        IFormattedLogValuesObfuscator formattedLogValuesObfuscator,
        ILogEventBuffer logEventBuffer)
    {
        _defaultLogger = defaultLogger;
        _secretObfuscator = secretObfuscator;
        _consoleWriter = consoleWriter;
        _collapsableLoggingStringBlockProvider = collapsableLoggingStringBlockProvider;
        _exceptionBuffer = exceptionBuffer;
        _formattedLogValuesObfuscator = formattedLogValuesObfuscator;
        _logEventBuffer = logEventBuffer;
        moduleLoggerContainer.AddLogger(this);

        Disposer.RegisterOnShutdown(this);
    }

    ~ModuleLogger()
    {
        Dispose();
    }

    public override IDisposable? BeginScope<TState>(TState state)
    {
        return new NoopDisposable();
    }

    public override bool IsEnabled(LogLevel logLevel)
    {
        return _defaultLogger.IsEnabled(logLevel);
    }

    public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string>? formatter)
    {
        lock (LogLock)
        {
            if (!IsEnabled(logLevel) || _isDisposed)
            {
                return;
            }

            _formattedLogValuesObfuscator.TryObfuscateValues(state!);

            var mappedFormatter = MapFormatter(formatter);

            var valueTuple = (logLevel, eventId, state, exception, mappedFormatter);

            _logEventBuffer.Add(valueTuple!);

            LastLogWritten = DateTime.UtcNow;
        }
    }

    public override void Dispose()
    {
        lock (DisposeLock)
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            if (_logEventBuffer.HasEvents)
            {
                var logEvents = _logEventBuffer.GetAndClear();

                PrintStartBlock();

                foreach (var stringOrLogEvent in logEvents)
                {
                    if (stringOrLogEvent.IsString)
                    {
                        _consoleWriter.LogToConsole(stringOrLogEvent.StringValue ?? string.Empty);
                    }
                    else if (stringOrLogEvent.LogEvent != null)
                    {
                        Log(stringOrLogEvent.LogEvent.Value);
                    }
                }

                PrintEndBlock();
            }

            GC.SuppressFinalize(this);
        }
    }

    public override void LogToConsole(string value)
    {
        _logEventBuffer.Add(_secretObfuscator.Obfuscate(value, null));
    }

    private void PrintStartBlock()
    {
        var startConsoleLogGroup =
            _collapsableLoggingStringBlockProvider.GetStartConsoleLogGroup(GetCollapsibleSectionName());

        if (startConsoleLogGroup != null)
        {
            _consoleWriter.LogToConsole(startConsoleLogGroup);
        }
    }

    private void PrintEndBlock()
    {
        var endConsoleLogGroup = _collapsableLoggingStringBlockProvider.GetEndConsoleLogGroup(GetCollapsibleSectionName());

        if (endConsoleLogGroup != null)
        {
            _consoleWriter.LogToConsole(endConsoleLogGroup);
        }
    }

    private void Log((LogLevel LogLevel, EventId EventId, object State, Exception? Exception, Func<object, Exception?, string> Formatter) logEvent)
    {
        var (logLevel, eventId, state, exception, formatter) = logEvent;

        try
        {
            _defaultLogger.Log(logLevel, eventId, state, exception, formatter);
        }
        catch (Exception e)
        {
            _exceptionBuffer.AddExceptionMessage(e.ToString());
            _exceptionBuffer.AddExceptionMessage($"{logLevel}: {eventId} - {state}{exception}");
        }
    }


    private Func<object, Exception?, string> MapFormatter<TState>(Func<TState, Exception?, string>? formatter)
    {
        if (formatter is null)
        {
            return (_, _) => string.Empty;
        }

        return (o, exception) =>
        {
            var formattedString = formatter.Invoke((TState) o, exception);
            return _secretObfuscator.Obfuscate(formattedString, null) ?? string.Empty;
        };
    }

    private string GetCollapsibleSectionName()
    {
        if (_exception != null)
        {
            return $"{typeof(T).Name} - Error! {_exception.GetType().Name}";
        }

        return $"{typeof(T).Name}";
    }
}