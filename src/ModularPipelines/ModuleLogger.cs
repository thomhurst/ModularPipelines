using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;
using ModularPipelines.Options;

namespace ModularPipelines;

internal abstract class ModuleLogger : ILogger, IDisposable
{
    internal DateTime LastLogWritten { get; set; } = DateTime.MinValue;
    public abstract void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter);
    public abstract bool IsEnabled(LogLevel logLevel);
    public abstract IDisposable BeginScope<TState>(TState state);
    public abstract void Dispose();
}

internal class ModuleLogger<T> : ModuleLogger, ILogger<T>, IDisposable
{
    private readonly IOptions<PipelineOptions> _options;
    private readonly ILogger<T> _defaultLogger;
    private readonly ISecretObfuscator _secretObfuscator;
    private readonly IBuildSystemDetector _buildSystemDetector;
    private readonly IModuleStatusProvider _moduleStatusProvider;

    private static readonly object _lock = new object();

    private List<(LogLevel logLevel, EventId eventId, object state, Exception? exception, Func<object, Exception?, string> formatter)> _logEvents = new();

    private bool _isDisposed;

    // ReSharper disable once ContextualLoggerProblem
    public ModuleLogger(IOptions<PipelineOptions> options, 
        ILogger<T> defaultLogger, 
        IModuleLoggerContainer moduleLoggerContainer, 
        ISecretObfuscator secretObfuscator,
        IBuildSystemDetector buildSystemDetector,
        IModuleStatusProvider moduleStatusProvider)
    {
        _options = options;
        _defaultLogger = defaultLogger;
        _secretObfuscator = secretObfuscator;
        _buildSystemDetector = buildSystemDetector;
        _moduleStatusProvider = moduleStatusProvider;
        moduleLoggerContainer.AddLogger(this);
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

        var mappedFormatter = MapFormatter(formatter);

        var valueTuple = (logLevel, eventId, state, exception, mappedFormatter);

        _logEvents.Add(valueTuple!);

        LastLogWritten = DateTime.UtcNow;
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
            return _secretObfuscator.Obfuscate(formattedString, null);
        };
    }

    private class NoopDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
    
    public override void Dispose()
    {
        lock (_lock)
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            var logEvents = Interlocked.Exchange(ref _logEvents!,
                new List<(LogLevel logLevel, EventId eventId, object state, Exception exception,
                    Func<object, Exception?, string> formatter)>());

            if (logEvents.Any())
            {
                PrintCollapsibleSectionStart();

                foreach (var (logLevel, eventId, state, exception, formatter) in logEvents)
                {
                    _defaultLogger.Log(logLevel, eventId, state, exception, formatter);
                }

                PrintCollapsibleSectionEnd();
            }

            logEvents.Clear();
            _logEvents.Clear();

            GC.SuppressFinalize(this);
        }
    }

    private void PrintCollapsibleSectionStart()
    {
        if (_buildSystemDetector.IsRunningOnGitHubActions)
        {
            Console.WriteLine($@"::group::{GetCollapsibleSectionName()}");
        }
        
        if (_buildSystemDetector.IsRunningOnAzurePipelines)
        {
            Console.WriteLine($@"##[group]{GetCollapsibleSectionName()}");
        }
        
        if (_buildSystemDetector.IsRunningOnTeamCity)
        {
            Console.WriteLine($@"##teamcity[blockOpened name='{GetCollapsibleSectionName()}']");
        }
    }

    private void PrintCollapsibleSectionEnd()
    {
        if (_buildSystemDetector.IsRunningOnGitHubActions)
        {
            Console.WriteLine(@"::endgroup::");
        }
        
        if (_buildSystemDetector.IsRunningOnAzurePipelines)
        {
            Console.WriteLine(@"##[endgroup]");
        }
        
        if (_buildSystemDetector.IsRunningOnTeamCity)
        {
            Console.WriteLine($@"##teamcity[blockClosed name='{GetCollapsibleSectionName()}']");
        }
    }

    private string GetCollapsibleSectionName()
    {
        return $"{typeof(T).Name}";
    }
}
