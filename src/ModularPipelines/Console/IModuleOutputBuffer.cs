using MEL.Spectre;
using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;

namespace ModularPipelines.Console;

/// <summary>
/// Buffers all output for a single module.
/// </summary>
/// <remarks>
/// <para>
/// <b>Purpose:</b> Consolidates all module output into a single buffer:
/// logger output, Console.WriteLine interceptions, and explicit writes.
/// </para>
/// <para>
/// <b>Thread Safety:</b> All methods are thread-safe and can be called concurrently.
/// </para>
/// <para>
/// <b>Flush Behavior:</b> Buffers are flushed immediately when modules complete,
/// via the OutputCoordinator which ensures ordered output.
/// </para>
/// </remarks>
internal interface IModuleOutputBuffer
{
    /// <summary>
    /// Gets the module type this buffer belongs to.
    /// </summary>
    Type ModuleType { get; }

    /// <summary>
    /// Adds a plain string line to the buffer.
    /// Used for Console.WriteLine interceptions.
    /// </summary>
    /// <param name="message">The message to buffer.</param>
    void WriteLine(string message);

    /// <summary>
    /// Buffers a build-system group command while preserving its position relative to section output.
    /// Raw CI commands bypass Spectre when flushed; local headers retain normal rendering.
    /// </summary>
    /// <param name="formatter">The active build-system formatter.</param>
    /// <param name="command">The group command, or <see langword="null"/> when unsupported.</param>
    void WriteGroupCommand(IBuildSystemFormatter formatter, string? command);

    /// <summary>
    /// Adds a structured log event to the buffer.
    /// Used for ILogger calls.
    /// </summary>
    /// <param name="logEvent">The structured log event.</param>
    void AddLogEvent(IBufferedLogEvent logEvent);

    /// <summary>
    /// Sets the exception if the module failed.
    /// Used for section header formatting.
    /// </summary>
    /// <param name="exception">The exception that caused failure.</param>
    void SetException(Exception exception);

    /// <summary>
    /// Registers a one-shot fallback for a deferred completion flush failure.
    /// </summary>
    /// <param name="handler">Receives the output flush exception.</param>
    void SetDeferredFlushFailureHandler(Action<Exception> handler)
    {
    }

    /// <summary>
    /// Reports that deferred completion output could not be flushed.
    /// </summary>
    /// <param name="exception">The output flush exception.</param>
    void ReportDeferredFlushFailure(Exception exception)
    {
    }

    /// <summary>
    /// Gets a value indicating whether there is any output to flush.
    /// </summary>
    bool HasOutput { get; }

    /// <summary>
    /// Gets a value indicating whether a structured event needs another provider delivery attempt.
    /// </summary>
    bool HasStructuredDeliveryRetries => false;

    /// <summary>
    /// Gets a value indicating whether the owning module has completed.
    /// </summary>
    bool IsComplete { get; }

    /// <summary>
    /// Gets a value indicating whether completing the module requires a final flush.
    /// This remains true while an incremental flush owns output and until its final status is rendered.
    /// </summary>
    bool NeedsCompletionFlush { get; }

    /// <summary>
    /// Marks the owning module as complete so periodic flushing no longer selects it.
    /// </summary>
    void MarkComplete();

    /// <summary>
    /// Flushes buffered output to the console with CI formatting.
    /// </summary>
    /// <param name="console">The console to write to.</param>
    /// <param name="formatter">The CI-specific formatter for log groups.</param>
    /// <param name="logger">The logger for structured log output.</param>
    /// <param name="loggerControl">Coordinates log rendering with direct console writes.</param>
    /// <param name="flushKind">Whether this is an incremental or complete flush.</param>
    /// <param name="fallbackLoggers">Non-Spectre loggers that receive structured events during direct fallback.</param>
    /// <param name="cancellationToken">Cancellation token for the flush operation.</param>
    /// <returns>A task that completes when buffered output has been rendered.</returns>
    Task FlushToAsync(
        TextWriter console,
        IBuildSystemFormatter formatter,
        ILogger logger,
        ISpectreConsoleLoggerControl loggerControl,
        OutputFlushKind flushKind,
        IReadOnlyList<ILogger>? fallbackLoggers = null,
        CancellationToken cancellationToken = default);
}
