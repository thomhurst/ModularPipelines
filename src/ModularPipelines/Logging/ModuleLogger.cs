using System.Reflection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;

namespace ModularPipelines.Logging;

internal abstract class ModuleLogger : IModuleLogger
{
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
    private readonly ISecretProvider _secretProvider;
    private readonly IConsoleWriter _consoleWriter;
    private readonly ISmartCollapsableLoggingStringBlockProvider _collapsableLoggingStringBlockProvider;

    private List<StringOrLogEvent> _stringOrLogEvents = new();

    private bool _isDisposed;

    // ReSharper disable once ContextualLoggerProblem
    public ModuleLogger(ILogger<T> defaultLogger,
        IModuleLoggerContainer moduleLoggerContainer,
        ISecretObfuscator secretObfuscator,
        ISecretProvider secretProvider,
        IConsoleWriter consoleWriter,
        ISmartCollapsableLoggingStringBlockProvider collapsableLoggingStringBlockProvider)
    {
        _defaultLogger = defaultLogger;
        _secretObfuscator = secretObfuscator;
        _secretProvider = secretProvider;
        _consoleWriter = consoleWriter;
        _collapsableLoggingStringBlockProvider = collapsableLoggingStringBlockProvider;
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

            if (state?.GetType().FullName == "Microsoft.Extensions.Logging.FormattedLogValues")
            {
                TryObfuscateValues(state);
            }

            var mappedFormatter = MapFormatter(formatter);

            var valueTuple = (logLevel, eventId, state, exception, mappedFormatter);

            _stringOrLogEvents.Add(valueTuple!);

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

            var logEvents = Interlocked.Exchange(ref _stringOrLogEvents, new List<StringOrLogEvent>());

            if (logEvents.Any())
            {
                PrintStartBlock();

                foreach (var stringOrLogEvent in logEvents)
                {
                    if (stringOrLogEvent.IsString)
                    {
                        _consoleWriter.LogToConsole(stringOrLogEvent.StringValue ?? string.Empty);
                    }
                    else if (stringOrLogEvent.LogEvent != null)
                    {
                        var (logLevel, eventId, state, exception, formatter) = stringOrLogEvent.LogEvent.Value;
                        _defaultLogger.Log(logLevel, eventId, state, exception, formatter);
                    }
                }

                PrintEndBlock();

                logEvents.Clear();
                _stringOrLogEvents.Clear();
            }

            GC.SuppressFinalize(this);
        }
    }

    public override void LogToConsole(string value)
    {
        foreach (var secret in _secretProvider.Secrets)
        {
            if (value.Contains(secret))
            {
                value = value.Replace(secret, "**********");
            }
        }

        _stringOrLogEvents.Add(value);
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

    private void TryObfuscateValues(object state)
    {
        var objArrayNullable = state.GetType()
            .GetField("_values", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.GetValue(state) as object?[] ?? [];

        for (var index = 0; index < objArrayNullable.Length; index++)
        {
            var obj = objArrayNullable[index];
            if (obj is null)
            {
                continue;
            }

            var objString = obj.ToString() ?? string.Empty;
            foreach (var secret in _secretProvider.Secrets)
            {
                if (objString.Contains(secret))
                {
                    objArrayNullable[index] = objString.Replace(secret, "**********");
                }
            }
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

    private class StringOrLogEvent
    {
        public (LogLevel LogLevel, EventId EventId, object State, Exception? Exception, Func<object, Exception?, string> Formatter)? LogEvent
        {
            get;
            private init;
        }

        public string? StringValue { get; private init; }

        public bool IsString => StringValue != null;

        public static implicit operator StringOrLogEvent(string value) => new()
        {
            StringValue = value,
        };

        public static implicit operator StringOrLogEvent((LogLevel LogLevel, EventId EventId, object State, Exception? Exception, Func<object, Exception?, string> Formatter) value) => new()
        {
            LogEvent = value,
        };
    }
}