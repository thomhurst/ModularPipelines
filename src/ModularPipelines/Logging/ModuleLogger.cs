using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;

namespace ModularPipelines.Logging;

/// <summary>
/// Base class for module-specific loggers with ambient context support.
/// </summary>
/// <remarks>
/// This class uses AsyncLocal to provide ambient context for the current module's logger.
/// This allows File/Folder helpers and other utilities to access the logger without explicit parameter passing.
/// AsyncLocal is thread-safe and flows with async/await contexts, making it ideal for async module execution.
/// </remarks>
internal abstract class ModuleLogger : IModuleLogger, IConsoleWriter
{
    /// <summary>
    /// Ambient context storage for the current module's logger.
    /// Uses AsyncLocal to ensure proper async context flow while maintaining thread safety.
    /// </summary>
    /// <remarks>
    /// This static field is accessed by ModuleExecutor to set the logger context before module execution
    /// and by File/Folder helpers to retrieve the current logger for operation logging.
    /// </remarks>
    internal static readonly AsyncLocal<IModuleLogger?> Values = new();

    /// <summary>
    /// Gets the current logger from ambient context, or a null logger if none is set.
    /// </summary>
    internal static ILogger Current => (Values.Value as ILogger) ?? NullLogger.Instance;

    protected readonly object _disposeLock = new();
    protected readonly object _logLock = new();
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

internal class ModuleLogger<T> : ModuleLogger, IModuleLogger, IConsoleWriter, ILogger<T>
{
    private readonly ILogger<T> _defaultLogger;
    private readonly ISecretObfuscator _secretObfuscator;
    private readonly IFormattedLogValuesObfuscator _formattedLogValuesObfuscator;
    private readonly ILogEventBuffer _logEventBuffer;
    private readonly ILoggerLifecycleCoordinator _lifecycleCoordinator;

    private bool _isDisposed;

    // ReSharper disable once ContextualLoggerProblem
    public ModuleLogger(ILogger<T> defaultLogger,
        IModuleLoggerContainer moduleLoggerContainer,
        ISecretObfuscator secretObfuscator,
        IFormattedLogValuesObfuscator formattedLogValuesObfuscator,
        ILogEventBuffer logEventBuffer,
        ILoggerLifecycleCoordinator lifecycleCoordinator)
    {
        _defaultLogger = defaultLogger;
        _secretObfuscator = secretObfuscator;
        _formattedLogValuesObfuscator = formattedLogValuesObfuscator;
        _logEventBuffer = logEventBuffer;
        _lifecycleCoordinator = lifecycleCoordinator;
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
        lock (_logLock)
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
        lock (_disposeLock)
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            _lifecycleCoordinator.FlushAndDispose(typeof(T).Name, _exception, _defaultLogger);

            GC.SuppressFinalize(this);
        }
    }

    public override void LogToConsole(string value)
    {
        _logEventBuffer.Add(_secretObfuscator.Obfuscate(value, null));
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
}