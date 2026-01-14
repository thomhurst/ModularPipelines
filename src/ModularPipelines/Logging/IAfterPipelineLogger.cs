namespace ModularPipelines.Logging;

/// <summary>
/// Provides buffered logging capabilities for messages to be written after pipeline completion.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="IAfterPipelineLogger"/> extends <see cref="ISummaryLogger"/> to provide both
/// the legacy string-based API and the new structured logging API.
/// </para>
/// <para>
/// <b>Recommended usage:</b> Prefer using the <see cref="ISummaryLogger"/> methods
/// (<see cref="ISummaryLogger.Info(string)"/>, <see cref="ISummaryLogger.Success(string)"/>,
/// <see cref="ISummaryLogger.Warning(string)"/>, <see cref="ISummaryLogger.Error(string)"/>)
/// for new code, as they provide better structure and categorization.
/// </para>
/// <para>
/// <b>Example usage:</b>
/// </para>
/// <code>
/// // New API (preferred)
/// context.Summary.Info("Build completed");
/// context.Summary.KeyValue("Version", version);
/// context.Summary.Success("Artifacts", "Published successfully");
///
/// // Legacy API (still supported)
/// context.LogOnPipelineEnd($"Version: {version}");
/// </code>
/// </remarks>
/// <seealso cref="ISummaryLogger"/>
public interface IAfterPipelineLogger : ISummaryLogger
{
    /// <summary>
    /// Adds a log message to be written after the pipeline completes.
    /// </summary>
    /// <param name="value">The log message to buffer.</param>
    /// <remarks>
    /// This method is provided for backward compatibility. For new code, prefer using
    /// <see cref="ISummaryLogger.Info(string)"/> or other typed logging methods.
    /// </remarks>
    [Obsolete("Use Info(), Success(), Warning(), Error(), or KeyValue() methods instead for better structure. This method will be removed in a future version.")]
    void LogOnPipelineEnd(string value);

    /// <summary>
    /// Gets all buffered log messages as a single string.
    /// </summary>
    /// <returns>A string containing all buffered messages, formatted with log levels and categories.</returns>
    string GetOutput();
}