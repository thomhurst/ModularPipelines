using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Options;

namespace ModularPipelines.Logging;

internal abstract class ModuleLogger : IModuleLogger
{
    protected static readonly object Lock = new();

    internal DateTime LastLogWritten { get; set; } = DateTime.MinValue;

    public abstract void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter);

    public abstract bool IsEnabled(LogLevel logLevel);

    public abstract IDisposable BeginScope<TState>(TState state);

    public abstract void Dispose();

    public abstract void LogToConsole(string value);
}

internal class ModuleLogger<T> : ModuleLogger, IModuleLogger, ILogger<T>
{
    private readonly IOptions<PipelineOptions> _options;
    private readonly ILogger<T> _defaultLogger;
    private readonly ISecretObfuscator _secretObfuscator;
    private readonly IBuildSystemDetector _buildSystemDetector;
    private readonly ISecretProvider _secretProvider;
    private readonly IConsoleWriter _consoleWriter;

    private List<StringOrLogEvent> _stringOrLogEvents = new();

    private bool _isDisposed;

    // ReSharper disable once ContextualLoggerProblem
    public ModuleLogger(IOptions<PipelineOptions> options,
        ILogger<T> defaultLogger,
        IModuleLoggerContainer moduleLoggerContainer,
        ISecretObfuscator secretObfuscator,
        IBuildSystemDetector buildSystemDetector,
        ISecretProvider secretProvider,
        IConsoleWriter consoleWriter)
    {
        _options = options;
        _defaultLogger = defaultLogger;
        _secretObfuscator = secretObfuscator;
        _buildSystemDetector = buildSystemDetector;
        _secretProvider = secretProvider;
        _consoleWriter = consoleWriter;
        moduleLoggerContainer.AddLogger(this);

        Disposer.RegisterOnShutdown(this);
    }

    ~ModuleLogger()
    {
        Dispose();
    }

    public override IDisposable BeginScope<TState>(TState state)
    {
        return new NoopDisposable();
    }

    public override bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= _options.Value.LoggerOptions.LogLevel;
    }

    public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string>? formatter)
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

    public override void Dispose()
    {
        lock (Lock)
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            var logEvents = Interlocked.Exchange(ref _stringOrLogEvents, new List<StringOrLogEvent>());

            if (logEvents.Any())
            {
                PrintCollapsibleSectionStart();

                foreach (var stringOrLogEvent in logEvents)
                {
                    if (stringOrLogEvent.IsString)
                    {
                        _consoleWriter.LogToConsole(stringOrLogEvent.StringValue);
                    }
                    else if (stringOrLogEvent.LogEvent != null)
                    {
                        var (logLevel, eventId, state, exception, formatter) = stringOrLogEvent.LogEvent.Value;
                        _defaultLogger.Log(logLevel, eventId, state, exception, formatter);
                    }
                }

                PrintCollapsibleSectionEnd();

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

    private void TryObfuscateValues(object state)
    {
        var objArrayNullable = state.GetType()
            .GetField("_values", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.GetValue(state) as object?[] ?? Array.Empty<object>();

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
            var formattedString = formatter.Invoke((TState)o, exception);
            return _secretObfuscator.Obfuscate(formattedString, null);
        };
    }

    private void PrintCollapsibleSectionStart()
    {
        if (_buildSystemDetector.IsRunningOnGitHubActions)
        {
            _consoleWriter.LogToConsole(BuildSystemValues.GitHub.StartBlock(GetCollapsibleSectionName()));
        }

        if (_buildSystemDetector.IsRunningOnAzurePipelines)
        {
            _consoleWriter.LogToConsole(BuildSystemValues.AzurePipelines.StartBlock(GetCollapsibleSectionName()));
        }

        if (_buildSystemDetector.IsRunningOnTeamCity)
        {
            _consoleWriter.LogToConsole(BuildSystemValues.TeamCity.StartBlock(GetCollapsibleSectionName()));
        }
    }

    private void PrintCollapsibleSectionEnd()
    {
        if (_buildSystemDetector.IsRunningOnGitHubActions)
        {
            _consoleWriter.LogToConsole(BuildSystemValues.GitHub.EndBlock);
        }

        if (_buildSystemDetector.IsRunningOnAzurePipelines)
        {
            _consoleWriter.LogToConsole(BuildSystemValues.AzurePipelines.EndBlock);
        }

        if (_buildSystemDetector.IsRunningOnTeamCity)
        {
            _consoleWriter.LogToConsole(BuildSystemValues.TeamCity.EndBlock(GetCollapsibleSectionName()));
        }
    }

    private string GetCollapsibleSectionName()
    {
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