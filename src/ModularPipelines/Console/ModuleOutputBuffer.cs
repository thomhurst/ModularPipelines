using System.Diagnostics.CodeAnalysis;
using MEL.Spectre;
using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using Spectre.Console;

namespace ModularPipelines.Console;

/// <summary>
/// Buffers all output for a single module.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread Safety:</b> This class is thread-safe. All public methods use locking
/// to ensure safe concurrent access from multiple threads.
/// </para>
/// <para>
/// <b>Buffer Contents:</b> The buffer holds both plain string output (from Console.WriteLine
/// interceptions) and structured log events (from ILogger calls). Both are stored in
/// insertion order and flushed together.
/// </para>
/// </remarks>
[ExcludeFromCodeCoverage]
internal class ModuleOutputBuffer : IModuleOutputBuffer
{
    private readonly List<BufferedOutput> _outputs = new();
    private readonly object _lock = new();
    private readonly string _moduleName;
    private readonly DateTime _startTimeUtc;
    private Exception? _exception;

    /// <inheritdoc />
    public Type ModuleType { get; }

    /// <summary>
    /// Initializes a new buffer for the specified module type.
    /// </summary>
    /// <param name="moduleType">The module type.</param>
    public ModuleOutputBuffer(Type moduleType)
    {
        ModuleType = moduleType;
        _moduleName = moduleType.Name;
        _startTimeUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Initializes a buffer for unattributed output (not from any module).
    /// </summary>
    /// <param name="name">Display name for the buffer.</param>
    /// <param name="moduleType">Placeholder type.</param>
    internal ModuleOutputBuffer(string name, Type moduleType)
    {
        ModuleType = moduleType;
        _moduleName = name;
        _startTimeUtc = DateTime.UtcNow;
    }

    /// <inheritdoc />
    public void WriteLine(string message)
    {
        lock (_lock)
        {
            _outputs.Add(BufferedOutput.FromString(message));
        }
    }

    /// <inheritdoc />
    public void AddLogEvent(
        LogLevel level,
        EventId eventId,
        object state,
        Exception? exception,
        Func<object, Exception?, string> formatter)
    {
        lock (_lock)
        {
            _outputs.Add(BufferedOutput.FromLogEvent(level, eventId, state, exception, formatter));
        }
    }

    /// <inheritdoc />
    public void SetException(Exception exception)
    {
        lock (_lock)
        {
            _exception = exception;
        }
    }

    /// <inheritdoc />
    public bool HasOutput
    {
        get
        {
            lock (_lock)
            {
                return _outputs.Count > 0;
            }
        }
    }

    /// <inheritdoc />
    public Task FlushToAsync(
        TextWriter console,
        IBuildSystemFormatter formatter,
        ILogger logger,
        ISpectreConsoleLoggerControl loggerControl,
        CancellationToken cancellationToken = default)
    {
        List<BufferedOutput> outputs;
        Exception? exception;

        lock (_lock)
        {
            if (_outputs.Count == 0)
            {
                return Task.CompletedTask;
            }

            outputs = new List<BufferedOutput>(_outputs);
            _outputs.Clear();
            exception = _exception;
        }

        var directConsole = CreateDirectConsole(console);
        var renderedCount = 0;

        try
        {
            var synchronizationLock = loggerControl.SynchronizationLock;
            EnterSynchronizationLock(synchronizationLock, cancellationToken);
            try
            {
                var header = FormatHeader(exception);
                var startCommand = formatter.GetStartBlockCommand(header);
                var endCommand = formatter.GetEndBlockCommand(header);
                var groupStarted = false;
                var flushCompleted = false;

                try
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    // Keep the synchronization gate for the complete group. MEL.Spectre uses
                    // synchronous rendering, so unrelated logger calls cannot enter this group.
                    if (startCommand != null)
                    {
                        WriteDirect(directConsole, console, startCommand);
                        groupStarted = true;
                    }

                    foreach (var output in outputs)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        if (output.IsString)
                        {
                            WriteDirect(directConsole, console, output.StringValue);
                        }
                        else if (output.LogEvent.HasValue)
                        {
                            var logEvent = output.LogEvent.Value;

                            // Synchronous MEL.Spectre rendering preserves this buffer's position
                            // while other providers (for example file logging) still receive the event.
                            logger.Log(logEvent.Level, logEvent.EventId, logEvent.State, logEvent.Exception,
                                (state, ex) => logEvent.Formatter(state, ex));
                        }

                        renderedCount++;
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                    flushCompleted = true;
                }
                finally
                {
                    if (groupStarted && endCommand != null)
                    {
                        console.WriteLine(endCommand);
                    }

                    if (groupStarted || flushCompleted)
                    {
                        // Add blank line between module sections for visual separation.
                        console.WriteLine();
                    }
                }
            }
            finally
            {
                Monitor.Exit(synchronizationLock);
            }
        }
        catch
        {
            RestoreUnrenderedOutputs(outputs, renderedCount);
            throw;
        }

        return Task.CompletedTask;
    }

    private static void EnterSynchronizationLock(
        object synchronizationLock,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        while (!Monitor.TryEnter(synchronizationLock, millisecondsTimeout: 50))
        {
            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    private void RestoreUnrenderedOutputs(List<BufferedOutput> outputs, int renderedCount)
    {
        if (renderedCount >= outputs.Count)
        {
            return;
        }

        lock (_lock)
        {
            _outputs.InsertRange(0, outputs.Skip(renderedCount));
        }
    }

    private string FormatHeader(Exception? exception)
    {
        var duration = DateTime.UtcNow - _startTimeUtc;
        var durationText = duration.ToDisplayString();

        if (exception != null)
        {
            return $"{_moduleName} \u2717 ({durationText}) - {exception.GetType().Name}";
        }

        return $"{_moduleName} \u2713 ({durationText})";
    }

    private static IAnsiConsole CreateDirectConsole(TextWriter writer)
    {
        var console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = new AnsiConsoleOutput(writer),
        });
        console.Profile.Width = AnsiConsole.Profile.Width;
        console.Profile.Capabilities = AnsiConsole.Profile.Capabilities;
        return console;
    }

    private static void WriteDirect(
        IAnsiConsole directConsole,
        TextWriter console,
        string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        try
        {
            directConsole.MarkupLine(value);
        }
        catch (Exception)
        {
            // CI workflow commands and arbitrary output can contain brackets that are not Spectre markup.
            console.WriteLine(value);
        }
    }
}

/// <summary>
/// Represents either a string or a structured log event in the buffer.
/// </summary>
internal readonly struct BufferedOutput
{
    /// <summary>
    /// Gets the string value if this is a string output.
    /// </summary>
    public string? StringValue { get; private init; }

    /// <summary>
    /// Gets the log event if this is a structured log event.
    /// </summary>
    public LogEventData? LogEvent { get; private init; }

    /// <summary>
    /// Gets whether this is a string output.
    /// </summary>
    public bool IsString => StringValue != null;

    /// <summary>
    /// Creates a buffered output from a string.
    /// </summary>
    public static BufferedOutput FromString(string value) => new() { StringValue = value };

    /// <summary>
    /// Creates a buffered output from a log event.
    /// </summary>
    public static BufferedOutput FromLogEvent(
        LogLevel level,
        EventId eventId,
        object state,
        Exception? exception,
        Func<object, Exception?, string> formatter)
        => new() { LogEvent = new LogEventData(level, eventId, state, exception, formatter) };
}

/// <summary>
/// Holds structured log event data for deferred output.
/// </summary>
internal readonly struct LogEventData
{
    public LogLevel Level { get; }
    public EventId EventId { get; }
    public object State { get; }
    public Exception? Exception { get; }
    public Func<object, Exception?, string> Formatter { get; }

    public LogEventData(
        LogLevel level,
        EventId eventId,
        object state,
        Exception? exception,
        Func<object, Exception?, string> formatter)
    {
        Level = level;
        EventId = eventId;
        State = state;
        Exception = exception;
        Formatter = formatter;
    }
}
