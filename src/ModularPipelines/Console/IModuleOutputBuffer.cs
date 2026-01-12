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
/// <b>Flush Ordering:</b> Buffers are flushed in completion order (by CompletedAtUtc)
/// to maintain logical output sequence.
/// </para>
/// </remarks>
internal interface IModuleOutputBuffer
{
    /// <summary>
    /// Gets the module type this buffer belongs to.
    /// </summary>
    Type ModuleType { get; }

    /// <summary>
    /// Gets when the module completed (for ordering during flush).
    /// Null if not yet completed.
    /// </summary>
    DateTime? CompletedAtUtc { get; }

    /// <summary>
    /// Records completion time for flush ordering.
    /// Called when the module finishes execution.
    /// </summary>
    void MarkCompleted();

    /// <summary>
    /// Adds a plain string line to the buffer.
    /// Used for Console.WriteLine interceptions.
    /// </summary>
    /// <param name="message">The message to buffer.</param>
    void WriteLine(string message);

    /// <summary>
    /// Adds a structured log event to the buffer.
    /// Used for ILogger calls.
    /// </summary>
    /// <param name="level">Log level.</param>
    /// <param name="eventId">Event identifier.</param>
    /// <param name="state">Log state object.</param>
    /// <param name="exception">Optional exception.</param>
    /// <param name="formatter">Formatter function.</param>
    void AddLogEvent(
        LogLevel level,
        EventId eventId,
        object state,
        Exception? exception,
        Func<object, Exception?, string> formatter);

    /// <summary>
    /// Sets the exception if the module failed.
    /// Used for section header formatting.
    /// </summary>
    /// <param name="exception">The exception that caused failure.</param>
    void SetException(Exception exception);

    /// <summary>
    /// Gets whether there is any output to flush.
    /// </summary>
    bool HasOutput { get; }

    /// <summary>
    /// Flushes all buffered output to the console with CI formatting.
    /// </summary>
    /// <param name="console">The console to write to.</param>
    /// <param name="formatter">The CI-specific formatter for log groups.</param>
    /// <param name="logger">The logger for structured log output.</param>
    void FlushTo(TextWriter console, IBuildSystemFormatter formatter, ILogger logger);
}
