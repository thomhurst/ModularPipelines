using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Console;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Logging;
using ModularPipelines.Secrets;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ModularPipelines.Logging;

/// <summary>
/// Base class for module-specific loggers with ambient context support.
/// </summary>
/// <remarks>
/// This class uses AsyncLocal to provide ambient context for the current module's logger and type.
/// This allows FilePath/FolderPath helpers and other utilities to access the logger without explicit parameter passing.
/// AsyncLocal is thread-safe and flows with async/await contexts, making it ideal for async module execution.
/// </remarks>
internal abstract class ModuleLogger : IInternalModuleLogger, IConsoleWriter, IAsyncDisposable
{
    /// <summary>
    /// Ambient context storage for the current module's logger.
    /// Uses AsyncLocal to ensure proper async context flow while maintaining thread safety.
    /// </summary>
    /// <remarks>
    /// This static field is accessed by ModuleExecutor to set the logger context before module execution
    /// and by FilePath/FolderPath helpers to retrieve the current logger for operation logging.
    /// </remarks>
    internal static readonly AsyncLocal<IModuleLogger?> Values = new();

    /// <summary>
    /// Ambient context storage for the current module's type.
    /// Enables fast module type detection without stack trace inspection.
    /// </summary>
    internal static readonly AsyncLocal<Type?> CurrentModuleType = new();

    /// <summary>
    /// Gets the current logger from ambient context, or a null logger if none is set.
    /// </summary>
    internal static ILogger Current => (Values.Value as ILogger) ?? NullLogger.Instance;

    protected readonly object _disposeLock = new();
    protected Exception? _exception;
    protected ModuleStatus _status = ModuleStatus.Succeeded;
    protected bool _preserveBufferForDeferredExecution;

    public abstract void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter);

    public abstract bool IsEnabled(LogLevel logLevel);

    public abstract IDisposable? BeginScope<TState>(TState state)
        where TState : notnull;

    public abstract void Dispose();

    public abstract ValueTask DisposeAsync();

    public abstract void WriteLine(string value);

    public abstract void WriteMarkupLine(string value);

    public abstract void Write(IRenderable renderable);

    public void SetException(Exception exception)
    {
        _exception = exception;
    }

    public void SetStatus(ModuleStatus status)
    {
        _status = status;
    }

    public void PreserveBufferForDeferredExecution()
    {
        _preserveBufferForDeferredExecution = true;
    }
}

internal class ModuleLogger<T> : ModuleLogger, IInternalModuleLogger, IConsoleWriter, ILogger<T>
{
    private readonly ILogger<T> _defaultLogger;
    private readonly ISecretObfuscator _secretObfuscator;
    private readonly IFormattedLogValuesObfuscator _formattedLogValuesObfuscator;
    private readonly IModuleOutputBuffer _buffer;
    private readonly IOutputCoordinator _outputCoordinator;
    private readonly IAnsiConsole _ansiConsole;
    private readonly object _renderLock = new();
    private readonly StringWriter _renderWriter;
    private readonly IAnsiConsole _renderConsole;

    private volatile bool _isDisposed;

    // ReSharper disable once ContextualLoggerProblem
    public ModuleLogger(
        ILogger<T> defaultLogger,
        ISecretObfuscator secretObfuscator,
        IFormattedLogValuesObfuscator formattedLogValuesObfuscator,
        IConsoleCoordinator consoleCoordinator,
        IOutputCoordinator outputCoordinator,
        IAnsiConsole? ansiConsole = null)
    {
        _defaultLogger = defaultLogger;
        _secretObfuscator = secretObfuscator;
        _formattedLogValuesObfuscator = formattedLogValuesObfuscator;
        _buffer = consoleCoordinator.GetModuleBuffer(typeof(T));
        _outputCoordinator = outputCoordinator;
        _ansiConsole = ansiConsole ?? DelegatingAnsiConsole.Instance;
        _renderWriter = new StringWriter();
        _renderConsole = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = new AnsiConsoleOutput(_renderWriter),
        });
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
        if (!IsEnabled(logLevel))
        {
            return;
        }

        if (_isDisposed)
        {
            return;
        }

        var obfuscatedState = state is null
            ? null
            : _formattedLogValuesObfuscator.TryObfuscateValues(state);
        var logEvent = new BufferedLogEvent<TState>(
            logLevel,
            eventId,
            state,
            obfuscatedState,
            exception,
            formatter ?? (static (_, _) => string.Empty),
            _secretObfuscator);

        lock (_disposeLock)
        {
            if (_isDisposed)
            {
                return;
            }

            // Write to buffer for ordered module output during pipeline execution.
            // Output will be flushed to console and loggers when the module completes.
            _buffer.AddLogEvent(logEvent);
        }
    }

    public override void Dispose()
    {
        // Synchronous disposal - just mark as disposed without blocking on flush
        // Prefer using DisposeAsync() for proper async output flushing
        lock (_disposeLock)
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
            CompleteBuffer();
        }

        GC.SuppressFinalize(this);
    }

    public override async ValueTask DisposeAsync()
    {
        bool shouldFlush;

        lock (_disposeLock)
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
            shouldFlush = CompleteBuffer();
        }

        if (shouldFlush)
        {
            if (_exception is not null)
            {
                _buffer.SetDeferredFlushFailureHandler(LogFlushFailure);
            }

            // Flush output asynchronously without blocking
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            try
            {
                await _outputCoordinator.OnModuleCompletedAsync(_buffer, typeof(T), cts.Token).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cts.IsCancellationRequested)
            {
                LogFlushTimeout();
            }
            catch (Exception ex)
            {
                LogFlushFailure(ex);
            }
        }

        GC.SuppressFinalize(this);
    }

    private bool CompleteBuffer()
    {
        if (_preserveBufferForDeferredExecution)
        {
            return false;
        }

        if (_exception != null)
        {
            _buffer.SetException(_exception);
        }

        _buffer.SetStatus(_status);
        _buffer.MarkComplete();
        return true;
    }

    private void LogFlushTimeout()
    {
        if (_exception is not null)
        {
            _defaultLogger.LogError(
                ObfuscatedLogException.Create(_exception, _secretObfuscator),
                "Module {ModuleType} failed and its buffered output timed out after 30 seconds",
                typeof(T).Name);
            return;
        }

        _defaultLogger.LogWarning(
            "Module output handling timed out after 30 seconds for {ModuleType}. Some output may be lost.",
            typeof(T).Name);
    }

    private void LogFlushFailure(Exception flushException)
    {
        if (_exception is not null)
        {
            _defaultLogger.LogError(
                ObfuscatedLogException.Create(_exception, _secretObfuscator),
                "Module {ModuleType} failed and its buffered output could not be flushed",
                typeof(T).Name);
            return;
        }

        _defaultLogger.LogWarning(
            flushException,
            "Failed to flush module output during disposal for {ModuleType}",
            typeof(T).Name);
    }

    public override void WriteLine(string value)
    {
        var obfuscated = _secretObfuscator.Obfuscate(value, null) ?? value;
        _buffer.WriteRenderable(new Markup(Markup.Escape(obfuscated)), obfuscated);
    }

    public override void WriteMarkupLine(string value)
    {
        Markup markup;
        try
        {
            markup = ObfuscatedMarkup.Create(value, _secretObfuscator);
        }
        catch (InvalidOperationException)
        {
            WriteLine(value);
            return;
        }

        WriteRenderable(markup, appendNewLine: true);
    }

    public override void Write(IRenderable renderable)
    {
        WriteRenderable(renderable, appendNewLine: false);
    }

    private void WriteRenderable(IRenderable renderable, bool appendNewLine)
    {
        var obfuscatedRenderable = new SecretObfuscatedRenderable(renderable, _secretObfuscator);
        var renderWidth = _ansiConsole.Profile.Width;
        var snapshot = obfuscatedRenderable.Snapshot(
            RenderOptions.Create(_ansiConsole),
            renderWidth);
        string rendered;
        lock (_renderLock)
        {
            _renderConsole.Profile.Width = renderWidth;
            _renderWriter.GetStringBuilder().Clear();
            _renderConsole.Write(snapshot);
            rendered = _renderWriter.ToString();
        }

        _buffer.WriteRenderable(snapshot, rendered, appendNewLine);
    }
}
