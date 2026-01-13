namespace ModularPipelines.Logging;

/// <summary>
/// Provides buffered logging capabilities for messages to be written after pipeline completion.
/// </summary>
public interface IAfterPipelineLogger
{
    /// <summary>
    /// Adds a log message to be written after the pipeline completes.
    /// </summary>
    /// <param name="value">The log message to buffer.</param>
    void LogOnPipelineEnd(string value);

    /// <summary>
    /// Gets all buffered log messages as a single string.
    /// </summary>
    /// <returns>A string containing all buffered messages.</returns>
    string GetOutput();
}