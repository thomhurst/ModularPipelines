using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
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
    private readonly int _outputFlushThreshold;
    private readonly Action<IModuleOutputBuffer>? _requestIncrementalFlush;
    private readonly ConditionalWeakTable<TextWriter, IAnsiConsole> _directConsoles = new();
    private Exception? _exception;
    private bool _isComplete;
    private bool _isIncrementalFlushInProgress;
    private bool _hasRenderedIncrementalOutput;
    private bool _thresholdFlushRequested;

    /// <inheritdoc />
    public Type ModuleType { get; }

    /// <summary>
    /// Initialises a new instance of the <see cref="ModuleOutputBuffer"/> class
    /// for the specified module type.
    /// </summary>
    /// <param name="moduleType">The module type.</param>
    /// <param name="outputFlushThreshold">The output count that triggers an incremental flush, or zero to disable threshold flushing.</param>
    /// <param name="requestIncrementalFlush">Callback that requests an incremental flush.</param>
    public ModuleOutputBuffer(
        Type moduleType,
        int outputFlushThreshold = 0,
        Action<IModuleOutputBuffer>? requestIncrementalFlush = null)
        : this(moduleType.Name, moduleType, outputFlushThreshold, requestIncrementalFlush)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="ModuleOutputBuffer"/> class
    /// for unattributed output.
    /// </summary>
    /// <param name="name">Display name for the buffer.</param>
    /// <param name="moduleType">Placeholder type.</param>
    /// <param name="outputFlushThreshold">The output count that triggers an incremental flush, or zero to disable threshold flushing.</param>
    /// <param name="requestIncrementalFlush">Callback that requests an incremental flush.</param>
    internal ModuleOutputBuffer(
        string name,
        Type moduleType,
        int outputFlushThreshold = 0,
        Action<IModuleOutputBuffer>? requestIncrementalFlush = null)
    {
        ModuleType = moduleType;
        _moduleName = name;
        _startTimeUtc = DateTime.UtcNow;
        _outputFlushThreshold = outputFlushThreshold;
        _requestIncrementalFlush = requestIncrementalFlush;
    }

    /// <inheritdoc />
    public void WriteLine(string message)
    {
        AddOutput(BufferedOutput.FromString(message));
    }

    /// <inheritdoc />
    public void AddLogEvent(IBufferedLogEvent logEvent)
    {
        AddOutput(BufferedOutput.FromLogEvent(logEvent));
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
    public bool IsComplete
    {
        get
        {
            lock (_lock)
            {
                return _isComplete;
            }
        }
    }

    /// <inheritdoc />
    public bool NeedsCompletionFlush
    {
        get
        {
            lock (_lock)
            {
                return _outputs.Count > 0
                       || _isIncrementalFlushInProgress
                       || _hasRenderedIncrementalOutput;
            }
        }
    }

    /// <inheritdoc />
    public void MarkComplete()
    {
        lock (_lock)
        {
            _isComplete = true;
        }
    }

    /// <inheritdoc />
    public Task FlushToAsync(
        TextWriter console,
        IBuildSystemFormatter formatter,
        ILogger logger,
        ISpectreConsoleLoggerControl loggerControl,
        OutputFlushKind flushKind,
        CancellationToken cancellationToken = default)
    {
        if (!TryTakeOutputs(flushKind, out var outputs, out var exception))
        {
            return Task.CompletedTask;
        }

        var directConsole = GetDirectConsole(console);
        var renderedCount = 0;

        try
        {
            RenderOutputs(
                console,
                directConsole,
                formatter,
                logger,
                loggerControl.SynchronizationLock,
                exception,
                flushKind,
                outputs,
                ref renderedCount,
                cancellationToken);
        }
        catch
        {
            if (flushKind is OutputFlushKind.Incremental)
            {
                RecordRenderedOutput(OutputFlushKind.Incremental, renderedCount);
            }

            RestoreUnrenderedOutputs(outputs, renderedCount);
            throw;
        }

        RecordRenderedOutput(flushKind, renderedCount);
        return Task.CompletedTask;
    }

    internal IAnsiConsole GetDirectConsole(TextWriter writer)
    {
        return _directConsoles.GetValue(writer, static value => CreateDirectConsole(value));
    }

    private void AddOutput(BufferedOutput output)
    {
        Action<IModuleOutputBuffer>? requestIncrementalFlush = null;

        lock (_lock)
        {
            if (_isComplete)
            {
                return;
            }

            _outputs.Add(output);
            if (_requestIncrementalFlush is not null
                && _outputFlushThreshold > 0
                && _outputs.Count >= _outputFlushThreshold
                && !_thresholdFlushRequested)
            {
                _thresholdFlushRequested = true;
                requestIncrementalFlush = _requestIncrementalFlush;
            }
        }

        requestIncrementalFlush?.Invoke(this);
    }

    private bool TryTakeOutputs(
        OutputFlushKind flushKind,
        out List<BufferedOutput> outputs,
        out Exception? exception)
    {
        lock (_lock)
        {
            if (flushKind is OutputFlushKind.Incremental && _isComplete)
            {
                outputs = null!;
                exception = null;
                return false;
            }

            if (_outputs.Count == 0
                && (flushKind is OutputFlushKind.Incremental || !_hasRenderedIncrementalOutput))
            {
                outputs = null!;
                exception = null;
                return false;
            }

            outputs = new List<BufferedOutput>(_outputs);
            _outputs.Clear();
            _thresholdFlushRequested = false;
            _isIncrementalFlushInProgress = flushKind is OutputFlushKind.Incremental;
            exception = _exception;
            return true;
        }
    }

    private void RenderOutputs(
        TextWriter console,
        IAnsiConsole directConsole,
        IBuildSystemFormatter formatter,
        ILogger logger,
        object synchronizationLock,
        Exception? exception,
        OutputFlushKind flushKind,
        List<BufferedOutput> outputs,
        ref int renderedCount,
        CancellationToken cancellationToken)
    {
        EnterSynchronizationLock(synchronizationLock, cancellationToken);
        try
        {
            RenderOutputGroup(
                console,
                directConsole,
                formatter,
                logger,
                exception,
                flushKind,
                outputs,
                ref renderedCount,
                cancellationToken);
        }
        finally
        {
            Monitor.Exit(synchronizationLock);
        }
    }

    private void RenderOutputGroup(
        TextWriter console,
        IAnsiConsole directConsole,
        IBuildSystemFormatter formatter,
        ILogger logger,
        Exception? exception,
        OutputFlushKind flushKind,
        List<BufferedOutput> outputs,
        ref int renderedCount,
        CancellationToken cancellationToken)
    {
        var header = FormatHeader(exception, flushKind);
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

            RenderBufferedOutputs(
                console,
                directConsole,
                logger,
                outputs,
                ref renderedCount,
                cancellationToken);

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

    private static void RenderBufferedOutputs(
        TextWriter console,
        IAnsiConsole directConsole,
        ILogger logger,
        List<BufferedOutput> outputs,
        ref int renderedCount,
        CancellationToken cancellationToken)
    {
        foreach (var output in outputs)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (output.IsString)
            {
                WriteDirect(directConsole, console, output.StringValue);
            }
            else if (output.LogEvent is { } logEvent)
            {
                // Synchronous MEL.Spectre rendering preserves this buffer's position
                // while other providers (for example file logging) still receive the event.
                logEvent.WriteTo(logger);
            }

            // Advance only after the sink returns successfully. A sink that accepts
            // output and then throws may cause a duplicate on retry, but retaining
            // the item avoids guaranteed data loss when delivery never happened.
            renderedCount++;
        }
    }

    private void RecordRenderedOutput(OutputFlushKind flushKind, int renderedCount)
    {
        lock (_lock)
        {
            if (flushKind is OutputFlushKind.Complete)
            {
                _hasRenderedIncrementalOutput = false;
            }
            else
            {
                _isIncrementalFlushInProgress = false;
                if (renderedCount > 0)
                {
                    _hasRenderedIncrementalOutput = true;
                }
            }
        }
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

    private string FormatHeader(Exception? exception, OutputFlushKind flushKind)
    {
        var duration = DateTime.UtcNow - _startTimeUtc;
        var durationText = duration.ToDisplayString();

        if (exception != null)
        {
            return $"{_moduleName} \u2717 ({durationText}) - {exception.GetType().Name}";
        }

        return flushKind is OutputFlushKind.Complete
            ? $"{_moduleName} \u2713 ({durationText})"
            : $"{_moduleName} \u2026 ({durationText})";
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
    public IBufferedLogEvent? LogEvent { get; private init; }

    /// <summary>
    /// Gets a value indicating whether this output contains a string.
    /// </summary>
    public bool IsString => StringValue != null;

    /// <summary>
    /// Creates a buffered output from a string.
    /// </summary>
    public static BufferedOutput FromString(string value) => new() { StringValue = value };

    /// <summary>
    /// Creates a buffered output from a log event.
    /// </summary>
    public static BufferedOutput FromLogEvent(IBufferedLogEvent logEvent)
        => new() { LogEvent = logEvent };
}

/// <summary>
/// Holds structured log event data for deferred output.
/// </summary>
internal interface IBufferedLogEvent
{
    void WriteTo(ILogger logger);
}

/// <summary>
/// Holds generic structured log state and its original formatter for deferred output.
/// </summary>
internal sealed class BufferedLogEvent<TState> : IBufferedLogEvent
{
    private readonly LogLevel _level;
    private readonly EventId _eventId;
    private readonly TState _originalState;
    private readonly object _obfuscatedState;
    private readonly Exception? _exception;
    private readonly Func<TState, Exception?, string> _formatter;
    private readonly ISecretObfuscator _secretObfuscator;

    public BufferedLogEvent(
        LogLevel level,
        EventId eventId,
        TState originalState,
        object obfuscatedState,
        Exception? exception,
        Func<TState, Exception?, string> formatter,
        ISecretObfuscator secretObfuscator)
    {
        _level = level;
        _eventId = eventId;
        _originalState = originalState;
        _obfuscatedState = obfuscatedState;
        _exception = exception;
        _formatter = formatter;
        _secretObfuscator = secretObfuscator;
    }

    public void WriteTo(ILogger logger)
    {
        logger.Log(
            _level,
            _eventId,
            _obfuscatedState,
            _exception,
            Format);
    }

    private string Format(object state, Exception? exception)
    {
        var formatted = _formatter(_originalState, exception);
        return _secretObfuscator.Obfuscate(formatted, null) ?? string.Empty;
    }
}
