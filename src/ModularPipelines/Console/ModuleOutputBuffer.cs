using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using MEL.Spectre;
using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
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
    private static readonly TimeSpan DefaultRenderGateTimeout = TimeSpan.FromSeconds(1);
    private readonly List<BufferedOutput> _outputs = [];
    private readonly List<StructuredDeliveryRetry> _structuredDeliveryRetries = [];
    private readonly Lock _lock = new();
    private readonly string _moduleName;
    private readonly DateTime _startTimeUtc;
    private readonly int _outputFlushThreshold;
    private readonly TimeSpan _renderGateTimeout;
    private readonly Func<LogLevel, bool> _isSpectreEnabled;
    private readonly Action<IModuleOutputBuffer>? _requestIncrementalFlush;
    private readonly bool _showFailureHeaderWithoutOutput;
    private readonly bool _showSuccessMarker;
    private readonly ConditionalWeakTable<TextWriter, IAnsiConsole> _directConsoles = [];
    private Exception? _exception;
    private bool _isComplete;
    private bool _hasRenderedCompletionHeader;
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
    /// <param name="renderGateTimeout">Maximum time to wait for the Spectre logger render gate.</param>
    /// <param name="isSpectreEnabled">Determines whether Spectre would render a structured event level.</param>
    /// <param name="showFailureHeaderWithoutOutput">Whether a failed empty buffer renders a failure header.</param>
    public ModuleOutputBuffer(
        Type moduleType,
        int outputFlushThreshold = 0,
        Action<IModuleOutputBuffer>? requestIncrementalFlush = null,
        TimeSpan? renderGateTimeout = null,
        Func<LogLevel, bool>? isSpectreEnabled = null,
        bool showFailureHeaderWithoutOutput = false)
        : this(
            moduleType.Name,
            moduleType,
            outputFlushThreshold,
            requestIncrementalFlush,
            renderGateTimeout,
            isSpectreEnabled,
            showFailureHeaderWithoutOutput)
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
    /// <param name="renderGateTimeout">Maximum time to wait for the Spectre logger render gate.</param>
    /// <param name="isSpectreEnabled">Determines whether Spectre would render a structured event level.</param>
    /// <param name="showFailureHeaderWithoutOutput">Whether a failed empty buffer renders a failure header.</param>
    /// <param name="showSuccessMarker">Whether successful output groups include a success marker.</param>
    internal ModuleOutputBuffer(
        string name,
        Type moduleType,
        int outputFlushThreshold = 0,
        Action<IModuleOutputBuffer>? requestIncrementalFlush = null,
        TimeSpan? renderGateTimeout = null,
        Func<LogLevel, bool>? isSpectreEnabled = null,
        bool showFailureHeaderWithoutOutput = false,
        bool showSuccessMarker = true)
    {
        ModuleType = moduleType;
        _moduleName = name;
        _startTimeUtc = DateTime.UtcNow;
        _outputFlushThreshold = outputFlushThreshold;
        _requestIncrementalFlush = requestIncrementalFlush;
        _renderGateTimeout = renderGateTimeout ?? DefaultRenderGateTimeout;
        _isSpectreEnabled = isSpectreEnabled ?? (static _ => true);
        _showFailureHeaderWithoutOutput = showFailureHeaderWithoutOutput;
        _showSuccessMarker = showSuccessMarker;
    }

    /// <inheritdoc />
    public void WriteLine(string message)
    {
        AddOutput(BufferedOutput.FromString(message), allowAfterCompletion: true);
    }

    /// <inheritdoc />
    public void WriteGroupCommand(IBuildSystemFormatter formatter, string? command)
    {
        formatter.WriteGroupCommand(
            command,
            value => AddOutput(
                BufferedOutput.FromRawBuildSystemCommand(value),
                allowAfterCompletion: true),
            WriteLine);
    }

    /// <inheritdoc />
    public void AddLogEvent(IBufferedLogEvent logEvent)
    {
        AddOutput(BufferedOutput.FromLogEvent(logEvent), allowAfterCompletion: false);
    }

    /// <inheritdoc />
    public void SetException(Exception exception)
    {
        lock (_lock)
        {
            _exception = exception;
            _hasRenderedCompletionHeader = false;
        }
    }

    /// <inheritdoc />
    public bool HasOutput
    {
        get
        {
            lock (_lock)
            {
                return _outputs.Count > 0 || _structuredDeliveryRetries.Count > 0;
            }
        }
    }

    /// <inheritdoc />
    public bool HasStructuredDeliveryRetries
    {
        get
        {
            lock (_lock)
            {
                return _structuredDeliveryRetries.Count > 0;
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
                       || _structuredDeliveryRetries.Count > 0
                       || _isIncrementalFlushInProgress
                       || (_exception is not null
                           && (_hasRenderedIncrementalOutput || _showFailureHeaderWithoutOutput)
                           && !_hasRenderedCompletionHeader);
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
    public async Task FlushToAsync(
        TextWriter console,
        IBuildSystemFormatter formatter,
        ILogger logger,
        ISpectreConsoleLoggerControl loggerControl,
        OutputFlushKind flushKind,
        IReadOnlyList<ILogger>? fallbackLoggers = null,
        CancellationToken cancellationToken = default)
    {
        if (!TryTakeOutputs(
                flushKind,
                out var outputs,
                out var structuredDeliveryRetries,
                out var shouldRenderOutputGroup,
                out var isContinuation,
                out var exception))
        {
            return;
        }

        var directConsole = GetDirectConsole(console);
        var effectiveFallbackLoggers = fallbackLoggers ?? [];
        var failedStructuredDeliveries = new List<StructuredDeliveryRetry>();
        var renderedCount = 0;

        try
        {
            RetryStructuredDeliveries(
                structuredDeliveryRetries,
                failedStructuredDeliveries,
                console,
                cancellationToken);

            if (shouldRenderOutputGroup)
            {
                using var renderGate = await loggerControl
                    .TryAcquireRenderGateAsync(_renderGateTimeout, cancellationToken)
                    .ConfigureAwait(false);
                RenderOutputs(
                    console,
                    directConsole,
                    formatter,
                    logger,
                    loggerControl,
                    renderGate,
                    exception,
                    flushKind,
                    isContinuation,
                    outputs,
                    effectiveFallbackLoggers,
                    failedStructuredDeliveries,
                    ref renderedCount,
                    cancellationToken);
            }
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
        finally
        {
            RestoreStructuredDeliveryRetries(failedStructuredDeliveries);
        }

        RecordRenderedOutput(flushKind, renderedCount);
    }

    internal IAnsiConsole GetDirectConsole(TextWriter writer)
    {
        return _directConsoles.GetValue(writer, static value => CreateDirectConsole(value));
    }

    private void AddOutput(BufferedOutput output, bool allowAfterCompletion)
    {
        Action<IModuleOutputBuffer>? requestIncrementalFlush = null;

        lock (_lock)
        {
            if (_isComplete && !allowAfterCompletion)
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
        out List<StructuredDeliveryRetry> structuredDeliveryRetries,
        out bool shouldRenderOutputGroup,
        out bool isContinuation,
        out Exception? exception)
    {
        lock (_lock)
        {
            if (flushKind is OutputFlushKind.Incremental && _isComplete)
            {
                outputs = null!;
                structuredDeliveryRetries = null!;
                shouldRenderOutputGroup = false;
                isContinuation = false;
                exception = null;
                return false;
            }

            var needsExceptionHeader = flushKind is OutputFlushKind.Complete
                                       && _exception is not null
                                       && (_hasRenderedIncrementalOutput || _showFailureHeaderWithoutOutput)
                                       && !_hasRenderedCompletionHeader;
            if (_outputs.Count == 0
                && _structuredDeliveryRetries.Count == 0
                && !needsExceptionHeader)
            {
                if (flushKind is OutputFlushKind.Complete)
                {
                    _hasRenderedIncrementalOutput = false;
                }

                outputs = null!;
                structuredDeliveryRetries = null!;
                shouldRenderOutputGroup = false;
                isContinuation = false;
                exception = null;
                return false;
            }

            outputs = [.. _outputs];
            structuredDeliveryRetries = [.. _structuredDeliveryRetries];
            shouldRenderOutputGroup = _outputs.Count > 0 || needsExceptionHeader;
            isContinuation = _hasRenderedIncrementalOutput;
            _outputs.Clear();
            _structuredDeliveryRetries.Clear();
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
        ISpectreConsoleLoggerControl loggerControl,
        IDisposable? renderGate,
        Exception? exception,
        OutputFlushKind flushKind,
        bool isContinuation,
        List<BufferedOutput> outputs,
        IReadOnlyList<ILogger> fallbackLoggers,
        List<StructuredDeliveryRetry> failedStructuredDeliveries,
        ref int renderedCount,
        CancellationToken cancellationToken)
    {
        if (renderGate is null)
        {
            console.WriteLine(
                $"Timed out waiting for the console logger render gate for {_moduleName}; writing buffered output directly.");
            RenderOutputGroup(
                console,
                directConsole,
                formatter,
                logger,
                exception,
                flushKind,
                isContinuation,
                outputs,
                fallbackLoggers,
                failedStructuredDeliveries,
                writeStructuredLogsDirectly: true,
                ref renderedCount,
                cancellationToken);
            return;
        }

        lock (loggerControl.SynchronizationLock)
        {
            RenderOutputGroup(
                console,
                directConsole,
                formatter,
                logger,
                exception,
                flushKind,
                isContinuation,
                outputs,
                fallbackLoggers,
                failedStructuredDeliveries,
                writeStructuredLogsDirectly: false,
                ref renderedCount,
                cancellationToken);
        }
    }

    private void RenderOutputGroup(
        TextWriter console,
        IAnsiConsole directConsole,
        IBuildSystemFormatter formatter,
        ILogger logger,
        Exception? exception,
        OutputFlushKind flushKind,
        bool isContinuation,
        List<BufferedOutput> outputs,
        IReadOnlyList<ILogger> fallbackLoggers,
        List<StructuredDeliveryRetry> failedStructuredDeliveries,
        bool writeStructuredLogsDirectly,
        ref int renderedCount,
        CancellationToken cancellationToken)
    {
        var header = FormatHeader(exception, flushKind, isContinuation);
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
                WriteGroupCommand(console, directConsole, formatter, startCommand);
                groupStarted = true;
            }

            RenderBufferedOutputs(
                console,
                directConsole,
                logger,
                outputs,
                fallbackLoggers,
                failedStructuredDeliveries,
                writeStructuredLogsDirectly,
                ref renderedCount,
                cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();
            flushCompleted = true;
        }
        finally
        {
            if (groupStarted && endCommand != null)
            {
                WriteGroupCommand(console, directConsole, formatter, endCommand);
            }

            if (groupStarted || flushCompleted)
            {
                // Add blank line between module sections for visual separation.
                console.WriteLine();
            }
        }
    }

    private void RenderBufferedOutputs(
        TextWriter console,
        IAnsiConsole directConsole,
        ILogger logger,
        List<BufferedOutput> outputs,
        IReadOnlyList<ILogger> fallbackLoggers,
        List<StructuredDeliveryRetry> failedStructuredDeliveries,
        bool writeStructuredLogsDirectly,
        ref int renderedCount,
        CancellationToken cancellationToken)
    {
        foreach (var output in outputs)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (output.IsRawBuildSystemCommand)
            {
                console.WriteLine(output.StringValue);
            }
            else if (output.IsString)
            {
                WriteDirect(directConsole, console, output.StringValue);
            }
            else if (output.LogEvent is { } logEvent)
            {
                if (writeStructuredLogsDirectly)
                {
                    var failedLoggers = WriteToFallbackLoggers(
                        logEvent,
                        fallbackLoggers,
                        console);
                    if (failedLoggers.Count > 0)
                    {
                        failedStructuredDeliveries.Add(
                            new StructuredDeliveryRetry(logEvent, failedLoggers));
                    }

                    if (_isSpectreEnabled(logEvent.Level))
                    {
                        WriteDirect(directConsole, console, logEvent.FormatMessageWithLevel());
                        if (logEvent.FormatException() is { } formattedException)
                        {
                            console.WriteLine(formattedException);
                        }
                    }
                }
                else
                {
                    // Synchronous MEL.Spectre rendering preserves this buffer's position
                    // while other providers (for example file logging) still receive the event.
                    logEvent.WriteTo(logger);
                }
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
                _hasRenderedCompletionHeader = true;
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

    private static IReadOnlyList<ILogger> WriteToFallbackLoggers(
        IBufferedLogEvent logEvent,
        IReadOnlyList<ILogger> fallbackLoggers,
        TextWriter console)
    {
        List<ILogger>? failedLoggers = null;
        foreach (var fallbackLogger in fallbackLoggers)
        {
            try
            {
                logEvent.WriteTo(fallbackLogger);
            }
            catch (ProviderDeliveryException exception)
            {
                (failedLoggers ??= []).AddRange(exception.FailedLoggers);
                console.WriteLine(
                    $"A non-console logger failed while handling buffered output: {exception.Message}");
            }
            catch (Exception exception)
            {
                (failedLoggers ??= []).Add(fallbackLogger);
                console.WriteLine(
                    $"A non-console logger failed while handling buffered output: {exception.Message}");
            }
        }

        return failedLoggers ?? [];
    }

    private static void RetryStructuredDeliveries(
        List<StructuredDeliveryRetry> structuredDeliveryRetries,
        List<StructuredDeliveryRetry> failedStructuredDeliveries,
        TextWriter console,
        CancellationToken cancellationToken)
    {
        for (var index = 0; index < structuredDeliveryRetries.Count; index++)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                failedStructuredDeliveries.AddRange(structuredDeliveryRetries.Skip(index));
                cancellationToken.ThrowIfCancellationRequested();
            }

            var retry = structuredDeliveryRetries[index];
            if (WriteToFallbackLoggers(retry.LogEvent, retry.Loggers, console).Count > 0)
            {
                console.WriteLine(
                    "Structured delivery was abandoned after 2 failed attempts; the direct console copy was retained.");
            }
        }
    }

    private void RestoreStructuredDeliveryRetries(
        List<StructuredDeliveryRetry> structuredDeliveryRetries)
    {
        if (structuredDeliveryRetries.Count == 0)
        {
            return;
        }

        lock (_lock)
        {
            _structuredDeliveryRetries.InsertRange(0, structuredDeliveryRetries);
        }
    }

    private readonly record struct StructuredDeliveryRetry(
        IBufferedLogEvent LogEvent,
        IReadOnlyList<ILogger> Loggers);

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

    private string FormatHeader(
        Exception? exception,
        OutputFlushKind flushKind,
        bool isContinuation)
    {
        var duration = DateTime.UtcNow - _startTimeUtc;
        var durationText = duration.ToDisplayString();
        var continuationText = isContinuation ? " (continued)" : string.Empty;

        if (exception != null)
        {
            return $"{_moduleName} \u2717{continuationText} ({durationText}) - {exception.GetType().Name}";
        }

        if (flushKind is OutputFlushKind.Incremental)
        {
            return $"{_moduleName} \u2026{continuationText} ({durationText})";
        }

        return _showSuccessMarker
            ? $"{_moduleName} \u2713{continuationText} ({durationText})"
            : $"{_moduleName}{continuationText} ({durationText})";
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

    private static void WriteGroupCommand(
        TextWriter console,
        IAnsiConsole directConsole,
        IBuildSystemFormatter formatter,
        string command)
    {
        formatter.WriteGroupCommand(
            command,
            console.WriteLine,
            value => WriteDirect(directConsole, console, value));
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
    /// Gets a value indicating whether this string is a raw build-system command.
    /// </summary>
    public bool IsRawBuildSystemCommand { get; private init; }

    /// <summary>
    /// Creates a buffered output from a string.
    /// </summary>
    public static BufferedOutput FromString(string value) => new() { StringValue = value };

    /// <summary>
    /// Creates a buffered output from a raw build-system command.
    /// </summary>
    public static BufferedOutput FromRawBuildSystemCommand(string value) =>
        new()
        {
            StringValue = value,
            IsRawBuildSystemCommand = true,
        };

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
    LogLevel Level { get; }

    void WriteTo(ILogger logger);

    string FormatMessageWithLevel();

    string? FormatException();
}

/// <summary>
/// Holds generic structured log state and its original formatter for deferred output.
/// </summary>
internal sealed class BufferedLogEvent<TState>(
    LogLevel level,
    EventId eventId,
    TState originalState,
    object? obfuscatedState,
    Exception? exception,
    Func<TState, Exception?, string> formatter,
    ISecretObfuscator secretObfuscator) : IBufferedLogEvent
{
    private readonly Exception? _obfuscatedException =
        ObfuscatedLogException.Create(exception, secretObfuscator);
    private readonly Lazy<string> _rawFormattedMessage = new(
        () => formatter(originalState, exception),
        LazyThreadSafetyMode.ExecutionAndPublication);

    public LogLevel Level => level;

    public void WriteTo(ILogger logger)
    {
        if (obfuscatedState is null && originalState is null)
        {
            logger.Log<TState>(
                level,
                eventId,
                originalState,
                _obfuscatedException,
                FormatTyped);
            return;
        }

        if (obfuscatedState is TState typedState)
        {
            logger.Log(
                level,
                eventId,
                typedState,
                _obfuscatedException,
                FormatTyped);
            return;
        }

        logger.Log(
            level,
            eventId,
            obfuscatedState,
            _obfuscatedException,
            Format);
    }

    public string FormatMessageWithLevel() => $"[{FormatLevel(level)}] {Format(null, null)}";

    public string? FormatException()
        => _obfuscatedException is null
            ? null
            : Obfuscate(_obfuscatedException.ToString());

    private string Format(object? state, Exception? logException)
        => Obfuscate(_rawFormattedMessage.Value);

    private string Obfuscate(string value)
        => secretObfuscator.Obfuscate(value, null);

    private string FormatTyped(TState state, Exception? logException)
        => Format(state!, logException);

    private static string FormatLevel(LogLevel logLevel) =>
        logLevel switch
        {
            LogLevel.Trace => "TRCE",
            LogLevel.Debug => "DBUG",
            LogLevel.Information => "INFO",
            LogLevel.Warning => "WARN",
            LogLevel.Error => "ERRO",
            LogLevel.Critical => "CRIT",
            _ => "NONE",
        };
}
