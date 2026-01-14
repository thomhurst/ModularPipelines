namespace ModularPipelines.Logging;

/// <summary>
/// Provides a fluent API for logging summary messages that appear after pipeline completion.
/// </summary>
/// <remarks>
/// <para>
/// Use <see cref="ISummaryLogger"/> to log important information that should be prominently
/// displayed in the pipeline summary. This is ideal for:
/// </para>
/// <list type="bullet">
/// <item><description>Version numbers and build artifacts</description></item>
/// <item><description>Important metrics and statistics</description></item>
/// <item><description>Deployment URLs and endpoints</description></item>
/// <item><description>Warnings and errors that need visibility</description></item>
/// </list>
/// <para>
/// <b>Thread Safety:</b> All methods are thread-safe and can be called concurrently
/// from multiple modules without external synchronization.
/// </para>
/// <para>
/// <b>Example usage:</b>
/// </para>
/// <code>
/// // Simple information logging
/// context.Summary.Info("Build completed successfully");
///
/// // Categorized logging for grouped display
/// context.Summary.Success("Version", $"Built version {version}");
/// context.Summary.Info("Artifacts", $"Published to {artifactPath}");
///
/// // Warning and error logging
/// context.Summary.Warning("Some tests were skipped");
/// context.Summary.Error("Deployment to staging failed");
///
/// // Key-value pair logging
/// context.Summary.KeyValue("Version", version);
/// context.Summary.KeyValue("Commit", commitHash);
/// </code>
/// </remarks>
/// <seealso cref="IAfterPipelineLogger"/>
public interface ISummaryLogger
{
    /// <summary>
    /// Logs an informational message to the pipeline summary.
    /// </summary>
    /// <param name="message">The message to log.</param>
    void Info(string message);

    /// <summary>
    /// Logs an informational message with a category to the pipeline summary.
    /// </summary>
    /// <param name="category">The category for grouping related messages.</param>
    /// <param name="message">The message to log.</param>
    void Info(string category, string message);

    /// <summary>
    /// Logs a success message to the pipeline summary.
    /// </summary>
    /// <param name="message">The message to log.</param>
    void Success(string message);

    /// <summary>
    /// Logs a success message with a category to the pipeline summary.
    /// </summary>
    /// <param name="category">The category for grouping related messages.</param>
    /// <param name="message">The message to log.</param>
    void Success(string category, string message);

    /// <summary>
    /// Logs a warning message to the pipeline summary.
    /// </summary>
    /// <param name="message">The message to log.</param>
    void Warning(string message);

    /// <summary>
    /// Logs a warning message with a category to the pipeline summary.
    /// </summary>
    /// <param name="category">The category for grouping related messages.</param>
    /// <param name="message">The message to log.</param>
    void Warning(string category, string message);

    /// <summary>
    /// Logs an error message to the pipeline summary.
    /// </summary>
    /// <param name="message">The message to log.</param>
    void Error(string message);

    /// <summary>
    /// Logs an error message with a category to the pipeline summary.
    /// </summary>
    /// <param name="category">The category for grouping related messages.</param>
    /// <param name="message">The message to log.</param>
    void Error(string category, string message);

    /// <summary>
    /// Logs a key-value pair to the pipeline summary.
    /// </summary>
    /// <param name="key">The key/label for the value.</param>
    /// <param name="value">The value to display.</param>
    /// <remarks>
    /// Key-value pairs are formatted for easy scanning in the summary output.
    /// Use this for metrics, version numbers, paths, and other structured data.
    /// </remarks>
    void KeyValue(string key, string value);

    /// <summary>
    /// Logs a key-value pair with a category to the pipeline summary.
    /// </summary>
    /// <param name="category">The category for grouping related key-value pairs.</param>
    /// <param name="key">The key/label for the value.</param>
    /// <param name="value">The value to display.</param>
    void KeyValue(string category, string key, string value);

    /// <summary>
    /// Logs a message with a specific log level to the pipeline summary.
    /// </summary>
    /// <param name="level">The severity level of the message.</param>
    /// <param name="message">The message to log.</param>
    void Log(SummaryLogLevel level, string message);

    /// <summary>
    /// Logs a message with a specific log level and category to the pipeline summary.
    /// </summary>
    /// <param name="level">The severity level of the message.</param>
    /// <param name="category">The category for grouping related messages.</param>
    /// <param name="message">The message to log.</param>
    void Log(SummaryLogLevel level, string category, string message);

    /// <summary>
    /// Gets all log entries that have been recorded.
    /// </summary>
    /// <returns>A read-only collection of all log entries.</returns>
    IReadOnlyList<SummaryLogEntry> GetEntries();

    /// <summary>
    /// Gets log entries filtered by category.
    /// </summary>
    /// <param name="category">The category to filter by.</param>
    /// <returns>A read-only collection of log entries matching the category.</returns>
    IReadOnlyList<SummaryLogEntry> GetEntries(string category);
}
