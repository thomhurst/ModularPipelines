namespace ModularPipelines.Logging;

/// <summary>
/// Represents a single log entry for the pipeline summary.
/// </summary>
/// <param name="Level">The severity level of the log entry.</param>
/// <param name="Message">The log message.</param>
/// <param name="Category">Optional category for grouping related entries.</param>
/// <param name="Timestamp">The time when the entry was created.</param>
public sealed record SummaryLogEntry(
    SummaryLogLevel Level,
    string Message,
    string? Category,
    DateTimeOffset Timestamp)
{
    /// <summary>
    /// Creates a new <see cref="SummaryLogEntry"/> with the specified level and message.
    /// </summary>
    /// <param name="level">The severity level.</param>
    /// <param name="message">The log message.</param>
    /// <param name="category">Optional category for grouping.</param>
    /// <returns>A new <see cref="SummaryLogEntry"/> instance.</returns>
    public static SummaryLogEntry Create(SummaryLogLevel level, string message, string? category = null)
        => new(level, message, category, DateTimeOffset.UtcNow);
}
