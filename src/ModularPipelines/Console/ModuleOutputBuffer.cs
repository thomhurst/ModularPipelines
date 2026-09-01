using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using MEL.Spectre;
using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Reporting;
using ModularPipelines.Secrets;
using Spectre.Console;
using Spectre.Console.Rendering;

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
internal class ModuleOutputBuffer : IModuleOutputBuffer, IPreObfuscatedModuleOutputBuffer
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
    private readonly ISecretObfuscator? _renderableSecretObfuscator;
    private readonly ISecretProvider? _renderableSecretProvider;
    private readonly IAnsiConsole? _renderableConsole;
    private readonly ModuleOutputExcerptBuffer? _outputExcerptBuffer;
    private readonly ConditionalWeakTable<TextWriter, IAnsiConsole> _directConsoles = [];
    private Exception? _exception;
    private Action<Exception>? _deferredFlushFailureHandler;
    private ModuleStatus _status = ModuleStatus.Succeeded;
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
    /// <param name="outputExcerptMaximumBytes">Maximum retained UTF-8 bytes for report output, or zero to disable capture.</param>
    /// <param name="outputExcerptSecretObfuscator">Obfuscator used before the final excerpt tail is selected.</param>
    /// <param name="outputExcerptSecretProvider">Provider used to validate late-registered secret boundaries.</param>
    /// <param name="renderableSecretObfuscator">Obfuscator used to mask rich output again immediately before emission.</param>
    /// <param name="renderableSecretProvider">Provider used to stabilize secrets while rich output is emitted.</param>
    /// <param name="renderableConsole">Console whose profile controls rich output layout.</param>
    public ModuleOutputBuffer(
        Type moduleType,
        int outputFlushThreshold = 0,
        Action<IModuleOutputBuffer>? requestIncrementalFlush = null,
        TimeSpan? renderGateTimeout = null,
        Func<LogLevel, bool>? isSpectreEnabled = null,
        bool showFailureHeaderWithoutOutput = false,
        int outputExcerptMaximumBytes = 0,
        ISecretObfuscator? outputExcerptSecretObfuscator = null,
        ISecretProvider? outputExcerptSecretProvider = null,
        ISecretObfuscator? renderableSecretObfuscator = null,
        ISecretProvider? renderableSecretProvider = null,
        IAnsiConsole? renderableConsole = null)
        : this(
            moduleType.Name,
            moduleType,
            outputFlushThreshold,
            requestIncrementalFlush,
            renderGateTimeout,
            isSpectreEnabled,
            showFailureHeaderWithoutOutput,
            outputExcerptMaximumBytes: outputExcerptMaximumBytes,
            outputExcerptSecretObfuscator: outputExcerptSecretObfuscator,
            outputExcerptSecretProvider: outputExcerptSecretProvider,
            renderableSecretObfuscator: renderableSecretObfuscator,
            renderableSecretProvider: renderableSecretProvider,
            renderableConsole: renderableConsole)
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
    /// <param name="outputExcerptMaximumBytes">Maximum retained UTF-8 bytes for report output, or zero to disable capture.</param>
    /// <param name="outputExcerptSecretObfuscator">Obfuscator used before the final excerpt tail is selected.</param>
    /// <param name="outputExcerptSecretProvider">Provider used to validate late-registered secret boundaries.</param>
    /// <param name="outputExcerptLogger">Logger for fail-closed excerpt diagnostics.</param>
    /// <param name="renderableSecretObfuscator">Obfuscator used to mask rich output again immediately before emission.</param>
    /// <param name="renderableSecretProvider">Provider used to stabilize secrets while rich output is emitted.</param>
    /// <param name="renderableConsole">Console whose profile controls rich output layout.</param>
    internal ModuleOutputBuffer(
        string name,
        Type moduleType,
        int outputFlushThreshold = 0,
        Action<IModuleOutputBuffer>? requestIncrementalFlush = null,
        TimeSpan? renderGateTimeout = null,
        Func<LogLevel, bool>? isSpectreEnabled = null,
        bool showFailureHeaderWithoutOutput = false,
        bool showSuccessMarker = true,
        int outputExcerptMaximumBytes = 0,
        ISecretObfuscator? outputExcerptSecretObfuscator = null,
        ISecretProvider? outputExcerptSecretProvider = null,
        ILogger? outputExcerptLogger = null,
        ISecretObfuscator? renderableSecretObfuscator = null,
        ISecretProvider? renderableSecretProvider = null,
        IAnsiConsole? renderableConsole = null)
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
        _renderableSecretObfuscator = renderableSecretObfuscator;
        _renderableSecretProvider = renderableSecretProvider;
        _renderableConsole = renderableConsole;
        _outputExcerptBuffer = outputExcerptMaximumBytes > 0
            ? new ModuleOutputExcerptBuffer(
                outputExcerptMaximumBytes,
                outputExcerptSecretObfuscator,
                outputExcerptSecretProvider,
                outputExcerptLogger)
            : null;
    }

    /// <inheritdoc />
    public void WriteLine(string message)
    {
        AddOutput(
            BufferedOutput.FromString(message, ModuleOutputStream.StandardOutput),
            allowAfterCompletion: true);
    }

    /// <inheritdoc />
    public void Write(string message)
    {
        AddOutput(
            BufferedOutput.FromString(
                message,
                ModuleOutputStream.StandardOutput,
                appendNewLine: false),
            allowAfterCompletion: true);
    }

    /// <inheritdoc />
    public void WriteRenderable(IRenderable renderable, string plainText)
    {
        WriteRenderable(renderable, plainText, appendNewLine: true);
    }

    /// <inheritdoc />
    public void WriteRenderable(IRenderable renderable, string plainText, bool appendNewLine)
    {
        AddOutput(
            BufferedOutput.FromRenderable(renderable, plainText, appendNewLine),
            allowAfterCompletion: true);
    }

    /// <inheritdoc />
    public void WriteErrorLine(string message)
    {
        AddOutput(
            BufferedOutput.FromString(message, ModuleOutputStream.StandardError),
            allowAfterCompletion: true);
    }

    /// <inheritdoc />
    public void WriteError(string message)
    {
        AddOutput(
            BufferedOutput.FromString(
                message,
                ModuleOutputStream.StandardError,
                appendNewLine: false),
            allowAfterCompletion: true);
    }

    /// <inheritdoc />
    public void WritePreObfuscated(
        string message,
        ModuleOutputStream stream,
        bool appendNewLine)
    {
        AddOutput(
            BufferedOutput.FromString(
                message,
                stream,
                appendNewLine,
                isPreObfuscated: true),
            allowAfterCompletion: true);
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
    public void SetDeferredFlushFailureHandler(Action<Exception> handler)
    {
        lock (_lock)
        {
            _deferredFlushFailureHandler = handler;
        }
    }

    /// <inheritdoc />
    public void ReportDeferredFlushFailure(Exception exception)
    {
        Action<Exception>? handler;
        lock (_lock)
        {
            handler = _deferredFlushFailureHandler;
            _deferredFlushFailureHandler = null;
        }

        try
        {
            handler?.Invoke(exception);
        }
        catch
        {
            // A diagnostic fallback must not replace the output failure.
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
    public void SetStatus(ModuleStatus status)
    {
        lock (_lock)
        {
            _status = status;
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
    public ModuleOutputExcerpt? GetOutputExcerpt()
    {
        lock (_lock)
        {
            return _outputExcerptBuffer?.CreateExcerpt();
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
        var effectiveFallbackLoggers = fallbackLoggers ?? [];
        var exclusiveSink = effectiveFallbackLoggers
            .OfType<IExclusiveStructuredLogSink>()
            .SingleOrDefault();
        var isStructuredLogEnabled = exclusiveSink is null
            ? _isSpectreEnabled
            : exclusiveSink.IsEnabled;
        if (!TryTakeOutputs(
                flushKind,
                isStructuredLogEnabled,
                effectiveFallbackLoggers,
                out var outputs,
                out var structuredDeliveryRetries,
                out var shouldRenderOutputGroup,
                out var isContinuation,
                out var exception))
        {
            return;
        }

        var directConsole = GetDirectConsole(console);
        var failedStructuredDeliveries = new List<StructuredDeliveryRetry>();
        var renderedCount = 0;
        var renderedConsoleOutput = false;

        try
        {
            RetryStructuredDeliveries(
                structuredDeliveryRetries,
                failedStructuredDeliveries,
                console,
                cancellationToken);

            if (shouldRenderOutputGroup
                || outputs.Any(static output => output.LogEvent is not null))
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
                    shouldRenderOutputGroup,
                    effectiveFallbackLoggers,
                    failedStructuredDeliveries,
                    ref renderedCount,
                    ref renderedConsoleOutput,
                    cancellationToken);
            }
        }
        catch
        {
            if (flushKind is OutputFlushKind.Incremental)
            {
                RecordRenderedOutput(OutputFlushKind.Incremental, renderedConsoleOutput);
            }

            RestoreUnrenderedOutputs(outputs, renderedCount);
            throw;
        }
        finally
        {
            RestoreStructuredDeliveryRetries(failedStructuredDeliveries);
        }

        RecordRenderedOutput(flushKind, renderedConsoleOutput);
    }

    internal IAnsiConsole GetDirectConsole(TextWriter writer)
    {
        var directConsole = _directConsoles.GetValue(writer, CreateDirectConsole);
        RefreshDirectConsoleProfile(directConsole);
        return directConsole;
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
            CaptureOutputExcerpt(output);
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

    private void CaptureOutputExcerpt(BufferedOutput output)
    {
        if (_outputExcerptBuffer is null || output.IsRawBuildSystemCommand)
        {
            return;
        }

        try
        {
            if (output.IsString)
            {
                _outputExcerptBuffer.Append(
                    output.StringValue!,
                    output.Stream,
                    output.AppendNewLine);
                return;
            }

            if (output.IsRenderable)
            {
                _outputExcerptBuffer.Append(
                    output.RenderablePlainText!,
                    output.Stream,
                    output.AppendNewLine);
                return;
            }

            if (output.LogEvent is not { } logEvent)
            {
                return;
            }

            _outputExcerptBuffer.Append(logEvent.FormatMessageWithLevel(), logEvent.Stream);
            if (logEvent.FormatException() is { } exception)
            {
                _outputExcerptBuffer.Append(exception, logEvent.Stream);
            }
        }
        catch (Exception)
        {
            // Report capture must never disrupt module logging or execution.
        }
    }

    private bool TryTakeOutputs(
        OutputFlushKind flushKind,
        Func<LogLevel, bool> isStructuredLogEnabled,
        IReadOnlyList<ILogger> fallbackLoggers,
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
            var flushableOutputCount = GetFlushableOutputCount(flushKind);
            if (flushableOutputCount == 0
                && _structuredDeliveryRetries.Count == 0
                && !needsExceptionHeader)
            {
                if (flushKind is OutputFlushKind.Complete)
                {
                    _hasRenderedIncrementalOutput = false;
                }

                _thresholdFlushRequested = false;
                outputs = null!;
                structuredDeliveryRetries = null!;
                shouldRenderOutputGroup = false;
                isContinuation = false;
                exception = null;
                return false;
            }

            outputs = _outputs.GetRange(0, flushableOutputCount);
            structuredDeliveryRetries = [.. _structuredDeliveryRetries];
            shouldRenderOutputGroup = needsExceptionHeader
                                      || outputs.Any(output => ProducesConsoleOutput(
                                          output,
                                          isStructuredLogEnabled,
                                          fallbackLoggers));
            isContinuation = _hasRenderedIncrementalOutput;
            _outputs.RemoveRange(0, flushableOutputCount);
            _structuredDeliveryRetries.Clear();
            _thresholdFlushRequested = false;
            _isIncrementalFlushInProgress = flushKind is OutputFlushKind.Incremental;
            exception = _exception;
            return true;
        }
    }

    private int GetFlushableOutputCount(OutputFlushKind flushKind)
    {
        if (flushKind is OutputFlushKind.Complete
            || _renderableSecretObfuscator is null)
        {
            return _outputs.Count;
        }

        var count = _outputs.Count;
        while (count > 0
               && IsMaskableOutput(_outputs[count - 1])
               && !_outputs[count - 1].AppendNewLine)
        {
            count--;
        }

        return count;
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
        bool shouldRenderOutputGroup,
        IReadOnlyList<ILogger> fallbackLoggers,
        List<StructuredDeliveryRetry> failedStructuredDeliveries,
        ref int renderedCount,
        ref bool renderedConsoleOutput,
        CancellationToken cancellationToken)
    {
        if (renderGate is null)
        {
            if (loggerControl is not NoopSpectreConsoleLoggerControl)
            {
                renderedConsoleOutput = true;
                console.WriteLine(
                    $"Timed out waiting for the console logger render gate for {_moduleName}; writing buffered output directly.");
            }

            RenderOutputGroup(
                console,
                directConsole,
                formatter,
                logger,
                exception,
                flushKind,
                isContinuation,
                outputs,
                shouldRenderOutputGroup,
                fallbackLoggers,
                failedStructuredDeliveries,
                writeStructuredLogsDirectly: true,
                ref renderedCount,
                ref renderedConsoleOutput,
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
                shouldRenderOutputGroup,
                fallbackLoggers,
                failedStructuredDeliveries,
                writeStructuredLogsDirectly: false,
                ref renderedCount,
                ref renderedConsoleOutput,
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
        bool shouldRenderOutputGroup,
        IReadOnlyList<ILogger> fallbackLoggers,
        List<StructuredDeliveryRetry> failedStructuredDeliveries,
        bool writeStructuredLogsDirectly,
        ref int renderedCount,
        ref bool renderedConsoleOutput,
        CancellationToken cancellationToken)
    {
        var header = FormatHeader(exception, flushKind, isContinuation);
        var startCommand = shouldRenderOutputGroup
            ? formatter.GetStartBlockCommand(header)
            : null;
        var endCommand = shouldRenderOutputGroup
            ? formatter.GetEndBlockCommand(header)
            : null;
        var groupStarted = false;
        var flushCompleted = false;

        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            renderedConsoleOutput |= shouldRenderOutputGroup;

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

            if (groupStarted || (flushCompleted && shouldRenderOutputGroup))
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
        for (var index = 0; index < outputs.Count;)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var renderedOutputCount = RenderBufferedOutput(
                console,
                directConsole,
                logger,
                outputs,
                index,
                fallbackLoggers,
                failedStructuredDeliveries,
                writeStructuredLogsDirectly);

            // Advance only after the sink returns successfully. A sink that accepts
            // output and then throws may cause a duplicate on retry, but retaining
            // the item avoids guaranteed data loss when delivery never happened.
            renderedCount += renderedOutputCount;
            index += renderedOutputCount;
        }
    }

    private int RenderBufferedOutput(
        TextWriter console,
        IAnsiConsole directConsole,
        ILogger logger,
        IReadOnlyList<BufferedOutput> outputs,
        int index,
        IReadOnlyList<ILogger> fallbackLoggers,
        List<StructuredDeliveryRetry> failedStructuredDeliveries,
        bool writeStructuredLogsDirectly)
    {
        var output = outputs[index];
        if (output.IsRawBuildSystemCommand)
        {
            console.WriteLine(output.StringValue);
            return 1;
        }

        var renderedMaskableOutputCount = TryRenderMaskableOutput(
            directConsole,
            outputs,
            index);
        if (renderedMaskableOutputCount > 0)
        {
            return renderedMaskableOutputCount;
        }

        if (output.IsString)
        {
            WriteDirect(directConsole, console, output.StringValue, output.AppendNewLine);
            return 1;
        }

        if (output.Renderable is { } renderable)
        {
            WriteRenderableWithCurrentSecrets(directConsole, renderable);
            if (output.AppendNewLine)
            {
                directConsole.WriteLine();
            }

            return 1;
        }

        if (output.LogEvent is not { } logEvent)
        {
            return 1;
        }

        if (writeStructuredLogsDirectly)
        {
            WriteStructuredLogDirectly(
                console,
                directConsole,
                logEvent,
                fallbackLoggers,
                failedStructuredDeliveries);
            return 1;
        }

        // Synchronous MEL.Spectre rendering preserves this buffer's position
        // while other providers (for example file logging) still receive the event.
        logEvent.WriteTo(logger);
        return 1;
    }

    private int TryRenderMaskableOutput(
        IAnsiConsole directConsole,
        IReadOnlyList<BufferedOutput> outputs,
        int index)
    {
        if (_renderableSecretObfuscator is null || !IsMaskableOutput(outputs[index]))
        {
            return 0;
        }

        var lastMaskableIndex = index;
        while (lastMaskableIndex + 1 < outputs.Count
               && IsMaskableOutput(outputs[lastMaskableIndex + 1]))
        {
            if (outputs[lastMaskableIndex].AppendNewLine
                && !HasPotentialSecretAcrossLineBoundary(outputs, index, lastMaskableIndex))
            {
                break;
            }

            lastMaskableIndex++;
        }

        BufferedOutput[] maskableOutputs =
        [
            .. outputs
                .Skip(index)
                .Take(lastMaskableIndex - index + 1),
        ];
        if (!maskableOutputs.Any(static output => output.IsRenderable)
            && maskableOutputs.All(static output => output.IsPreObfuscated))
        {
            return 0;
        }

        if (!maskableOutputs.Any(static output => output.IsRenderable)
            && !maskableOutputs.Any(static output => output.IsPreObfuscated))
        {
            WriteRawStringsWithCurrentSecrets(
                directConsole,
                GetMaskableSource(maskableOutputs));
            if (maskableOutputs[^1].AppendNewLine)
            {
                directConsole.WriteLine();
            }

            return maskableOutputs.Length;
        }

        var maskableRenderables = GetMaskableRenderables(maskableOutputs);
        var renderable = maskableRenderables.Count == 1
            ? maskableRenderables[0]
            : new ConcatenatedRenderable(
                maskableRenderables);
        WriteRenderableWithCurrentSecrets(
            directConsole,
            renderable,
            GetMixedOutputObfuscator(maskableOutputs));
        if (maskableOutputs[^1].AppendNewLine)
        {
            directConsole.WriteLine();
        }

        return maskableOutputs.Length;
    }

    private static bool IsMaskableOutput(BufferedOutput output) =>
        output.IsRenderable || (output.IsString && !output.IsRawBuildSystemCommand);

    private static IRenderable GetMaskableRenderable(BufferedOutput output) =>
        output.Renderable
        ?? (output.StringValue is { } value
            ? new BufferedStringRenderable(value)
            : throw new InvalidOperationException("Buffered output is not maskable."));

    private static List<IRenderable> GetMaskableRenderables(
        BufferedOutput[] outputs)
    {
        var renderables = new List<IRenderable>((outputs.Length * 2) - 1);
        for (var index = 0; index < outputs.Length; index++)
        {
            var output = outputs[index];
            renderables.Add(GetMaskableRenderable(output));
            if (output.AppendNewLine && index < outputs.Length - 1)
            {
                renderables.Add(new Text(Environment.NewLine));
            }
        }

        return renderables;
    }

    private static string GetMaskableSource(BufferedOutput[] outputs)
    {
        var source = new StringBuilder();
        for (var index = 0; index < outputs.Length; index++)
        {
            source.Append(GetMaskablePlainText(outputs[index]));
            if (outputs[index].AppendNewLine && index < outputs.Length - 1)
            {
                source.Append(Environment.NewLine);
            }
        }

        return source.ToString();
    }

    private bool HasPotentialSecretAcrossLineBoundary(
        IReadOnlyList<BufferedOutput> outputs,
        int firstIndex,
        int lastIndex)
    {
        if (_renderableSecretProvider is null)
        {
            return false;
        }

        var source = new StringBuilder();
        for (var index = firstIndex; index <= lastIndex; index++)
        {
            source.Append(GetMaskablePlainText(outputs[index]));
            if (outputs[index].AppendNewLine)
            {
                source.Append(Environment.NewLine);
            }
        }

        return GetPotentialSecretPrefixLength(source.ToString()) > 0;
    }

    private ISecretObfuscator? GetMixedOutputObfuscator(
        BufferedOutput[] outputs)
    {
        if (_renderableSecretObfuscator is null
            || _renderableSecretObfuscator is SecretObfuscator
            || outputs[0] is not { IsPreObfuscated: true, StringValue: { } value })
        {
            return _renderableSecretObfuscator;
        }

        var boundarySource = value + (outputs[0].AppendNewLine && outputs.Length > 1
            ? Environment.NewLine
            : string.Empty);
        var retainedLength = GetPotentialSecretPrefixLength(boundarySource);
        var protectedLength = Math.Clamp(
            boundarySource.Length - retainedLength,
            0,
            value.Length);
        return protectedLength == 0
            ? _renderableSecretObfuscator
            : new PrefixPreservingSecretObfuscator(
                _renderableSecretObfuscator,
                value[..protectedLength]);
    }

    private int GetPotentialSecretPrefixLength(string value)
    {
        if (_renderableSecretProvider is null || value.Length == 0)
        {
            return 0;
        }

        var comparison = _renderableSecretObfuscator is ITrackedSecretObfuscator tracked
            ? tracked.PatternComparison
            : StringComparison.OrdinalIgnoreCase;
        var retainedLength = 0;
        var secrets = _renderableSecretProvider.GetSnapshot().Secrets ?? [];
        foreach (var secret in secrets.Where(static secret => !string.IsNullOrEmpty(secret)))
        {
            var maximumLength = Math.Min(value.Length, secret.Length - 1);
            for (var length = maximumLength; length > 0; length--)
            {
                if (secret.AsSpan().StartsWith(value.AsSpan(value.Length - length), comparison))
                {
                    retainedLength = Math.Max(retainedLength, length);
                    break;
                }
            }
        }

        return retainedLength;
    }

    private static string GetMaskablePlainText(BufferedOutput output) =>
        output.StringValue ?? output.RenderablePlainText ?? string.Empty;

    private void WriteStructuredLogDirectly(
        TextWriter console,
        IAnsiConsole directConsole,
        IBufferedLogEvent logEvent,
        IReadOnlyList<ILogger> fallbackLoggers,
        List<StructuredDeliveryRetry> failedStructuredDeliveries)
    {
        var failedLoggers = WriteToFallbackLoggers(logEvent, fallbackLoggers, console);
        if (failedLoggers.Count > 0)
        {
            failedStructuredDeliveries.Add(new StructuredDeliveryRetry(logEvent, failedLoggers));
        }

        var wroteToDirectStructuredLogSink = fallbackLoggers.Any(logger =>
            logger is IDirectStructuredLogSink && !failedLoggers.Contains(logger));
        if (!_isSpectreEnabled(logEvent.Level) || wroteToDirectStructuredLogSink)
        {
            return;
        }

        WriteDirect(directConsole, console, logEvent.FormatMessageWithLevel());
        if (logEvent.FormatException() is { } formattedException)
        {
            console.WriteLine(formattedException);
        }
    }

    private void WriteRenderableWithCurrentSecrets(
        IAnsiConsole directConsole,
        IRenderable renderable,
        ISecretObfuscator? secretObfuscator = null)
    {
        secretObfuscator ??= _renderableSecretObfuscator;
        if (secretObfuscator is null)
        {
            directConsole.Write(renderable);
            return;
        }

        var remasked = renderable is SecretObfuscatedRenderable
        {
            RequiresPostRenderObfuscation: false,
        }
            ? renderable
            : new SecretObfuscatedRenderable(renderable, secretObfuscator);
        if (_renderableSecretProvider is ISecretEmissionGuard emissionGuard)
        {
            emissionGuard.ExecuteWithStableSecrets(
                (Console: directConsole, Renderable: remasked),
                static state => state.Console.Write(state.Renderable));
            return;
        }

        directConsole.Write(remasked);
    }

    private void WriteRawStringsWithCurrentSecrets(
        IAnsiConsole directConsole,
        string source)
    {
        if (_renderableSecretObfuscator is null)
        {
            directConsole.Markup(source);
            return;
        }

        if (_renderableSecretProvider is ISecretEmissionGuard emissionGuard)
        {
            emissionGuard.ExecuteWithStableSecrets(
                (Console: directConsole, Source: source, Obfuscator: _renderableSecretObfuscator),
                static state => state.Console.Write(
                    ObfuscatedMarkup.Create(state.Source, state.Obfuscator)));
            return;
        }

        directConsole.Write(ObfuscatedMarkup.Create(source, _renderableSecretObfuscator));
    }

    private void RecordRenderedOutput(OutputFlushKind flushKind, bool renderedConsoleOutput)
    {
        lock (_lock)
        {
            if (flushKind is OutputFlushKind.Complete)
            {
                _hasRenderedIncrementalOutput = false;
                _hasRenderedCompletionHeader = true;
                _deferredFlushFailureHandler = null;
            }
            else
            {
                _isIncrementalFlushInProgress = false;
                if (renderedConsoleOutput)
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

        if (!_showSuccessMarker)
        {
            return $"{_moduleName}{continuationText} ({durationText})";
        }

        ModuleStatus status;

        lock (_lock)
        {
            status = _status;
        }

        var completionMarker = Markup.Remove(StatusDisplayProvider.GetDisplayInfo(status).Icon);
        return $"{_moduleName} {completionMarker}{continuationText} ({durationText})";
    }

    private static bool ProducesConsoleOutput(
        BufferedOutput output,
        Func<LogLevel, bool> isStructuredLogEnabled,
        IReadOnlyList<ILogger> fallbackLoggers)
    {
        if (output.IsRawBuildSystemCommand)
        {
            return true;
        }

        if (output.IsString)
        {
            return !string.IsNullOrEmpty(output.StringValue);
        }

        if (output.IsRenderable)
        {
            return true;
        }

        if (output.LogEvent is not { } logEvent)
        {
            return false;
        }

        var foundBufferedConsoleLogger = false;
        foreach (var fallbackLogger in fallbackLoggers)
        {
            if (fallbackLogger is not IBufferedConsoleLogger bufferedConsoleLogger)
            {
                continue;
            }

            foundBufferedConsoleLogger = true;
            if (bufferedConsoleLogger.WouldWrite(logEvent))
            {
                return true;
            }
        }

        return !foundBufferedConsoleLogger && isStructuredLogEnabled(logEvent.Level);
    }

    private IAnsiConsole CreateDirectConsole(TextWriter writer)
    {
        return AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = new AnsiConsoleOutput(writer),
        });
    }

    private void RefreshDirectConsoleProfile(IAnsiConsole directConsole)
    {
        var sourceProfile = _renderableConsole?.Profile ?? AnsiConsole.Profile;
        directConsole.Profile.Width = sourceProfile.Width;
        directConsole.Profile.Height = sourceProfile.Height;
        directConsole.Profile.Capabilities = sourceProfile.Capabilities;
    }

    private static void WriteDirect(
        IAnsiConsole directConsole,
        TextWriter console,
        string? value,
        bool appendNewLine = true)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        try
        {
            if (appendNewLine)
            {
                directConsole.MarkupLine(value);
            }
            else
            {
                directConsole.Markup(value);
            }
        }
        catch (Exception)
        {
            // CI workflow commands and arbitrary output can contain brackets that are not Spectre markup.
            if (appendNewLine)
            {
                console.WriteLine(value);
            }
            else
            {
                console.Write(value);
            }
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

    private sealed class ConcatenatedRenderable(IReadOnlyList<IRenderable> renderables) : IRenderable
    {
        public Measurement Measure(RenderOptions options, int maxWidth)
        {
            var text = string.Concat(Render(options, maxWidth)
                .Where(static segment => !segment.IsControlCode)
                .Select(static segment => segment.Text));
            return ((IRenderable) new Text(text)).Measure(options, maxWidth);
        }

        public IEnumerable<Segment> Render(RenderOptions options, int maxWidth) =>
            renderables.SelectMany(renderable => renderable.Render(options, maxWidth));
    }

    private sealed class BufferedStringRenderable(string value) : IRenderable
    {
        public Measurement Measure(RenderOptions options, int maxWidth)
        {
            try
            {
                return ((IRenderable) new Markup(value)).Measure(options, maxWidth);
            }
            catch (Exception)
            {
                return ((IRenderable) new Text(value)).Measure(options, maxWidth);
            }
        }

        public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
        {
            try
            {
                return [.. ((IRenderable) new Markup(value)).Render(options, maxWidth)];
            }
            catch (Exception)
            {
                return [.. ((IRenderable) new Text(value)).Render(options, maxWidth)];
            }
        }
    }

    private sealed class PrefixPreservingSecretObfuscator(
        ISecretObfuscator inner,
        string protectedPrefix) : ISecretObfuscator
    {
        public bool HasSecrets => inner.HasSecrets;

        public string Obfuscate(string? input, object? optionsObject)
        {
            var output = inner.Obfuscate(input, optionsObject);
            var obfuscatedPrefix = inner.Obfuscate(protectedPrefix, optionsObject);
            return output.StartsWith(obfuscatedPrefix, StringComparison.Ordinal)
                ? protectedPrefix + output[obfuscatedPrefix.Length..]
                : output;
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
    /// Gets the rich renderable value, when present.
    /// </summary>
    public IRenderable? Renderable { get; private init; }

    /// <summary>
    /// Gets the plain-text representation retained for run reports.
    /// </summary>
    public string? RenderablePlainText { get; private init; }

    /// <summary>
    /// Gets a value indicating whether this output contains a string.
    /// </summary>
    public bool IsString => StringValue != null;

    /// <summary>
    /// Gets a value indicating whether this output contains a rich renderable.
    /// </summary>
    public bool IsRenderable => Renderable != null;

    /// <summary>
    /// Gets a value indicating whether this string is a raw build-system command.
    /// </summary>
    public bool IsRawBuildSystemCommand { get; private init; }

    /// <summary>
    /// Gets a value indicating whether console interception already obfuscated this string.
    /// </summary>
    public bool IsPreObfuscated { get; private init; }

    /// <summary>
    /// Gets the output stream represented by this item.
    /// </summary>
    public ModuleOutputStream Stream { get; private init; }

    /// <summary>
    /// Gets a value indicating whether a line terminator follows the string output.
    /// </summary>
    public bool AppendNewLine { get; private init; }

    /// <summary>
    /// Creates a buffered output from a string.
    /// </summary>
    public static BufferedOutput FromString(
        string value,
        ModuleOutputStream stream = ModuleOutputStream.StandardOutput,
        bool appendNewLine = true,
        bool isPreObfuscated = false) =>
        new()
        {
            StringValue = value,
            Stream = stream,
            AppendNewLine = appendNewLine,
            IsPreObfuscated = isPreObfuscated,
        };

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
    /// Creates buffered output from a rich renderable.
    /// </summary>
    public static BufferedOutput FromRenderable(
        IRenderable renderable,
        string plainText,
        bool appendNewLine = true) =>
        new()
        {
            Renderable = renderable,
            RenderablePlainText = plainText,
            Stream = ModuleOutputStream.StandardOutput,
            AppendNewLine = appendNewLine,
        };

    /// <summary>
    /// Creates a buffered output from a log event.
    /// </summary>
    public static BufferedOutput FromLogEvent(IBufferedLogEvent logEvent)
        => new() { LogEvent = logEvent, Stream = logEvent.Stream };
}

/// <summary>
/// Holds structured log event data for deferred output.
/// </summary>
internal interface IBufferedLogEvent
{
    LogLevel Level { get; }

    ModuleOutputStream Stream => ModuleOutputStream.StandardOutput;

    void WriteTo(ILogger logger);

    SynchronousConsoleLogEntry FormatFor(IBufferedConsoleLogger logger);

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

    public ModuleOutputStream Stream { get; } = GetStream(obfuscatedState);

    public void WriteTo(ILogger logger)
    {
        if (logger is IBufferedConsoleLogger bufferedConsoleLogger)
        {
            bufferedConsoleLogger.Write(this);
            return;
        }

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

    public SynchronousConsoleLogEntry FormatFor(IBufferedConsoleLogger logger)
    {
        if (obfuscatedState is null && originalState is null)
        {
            return logger.Format<TState>(
                level,
                eventId,
                originalState,
                _obfuscatedException,
                FormatTyped);
        }

        if (obfuscatedState is TState typedState)
        {
            return logger.Format(
                level,
                eventId,
                typedState,
                _obfuscatedException,
                FormatTyped);
        }

        return logger.Format(
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

    private static ModuleOutputStream GetStream(object? state)
    {
        if (state is not IReadOnlyList<KeyValuePair<string, object?>> properties)
        {
            return ModuleOutputStream.StandardOutput;
        }

        try
        {
            for (var index = 0; index < properties.Count; index++)
            {
                if (properties[index].Key == "CommandError")
                {
                    return ModuleOutputStream.StandardError;
                }
            }
        }
        catch (Exception exception) when (exception is not (OutOfMemoryException or StackOverflowException))
        {
            // Structured state is user-controlled. Classification is best-effort and
            // must not make an otherwise valid logging call fail.
        }

        return ModuleOutputStream.StandardOutput;
    }
}
