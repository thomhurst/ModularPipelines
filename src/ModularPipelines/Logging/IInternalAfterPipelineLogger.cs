namespace ModularPipelines.Logging;

/// <summary>
/// Internal interface for after-pipeline logger operations not exposed to consumers.
/// </summary>
internal interface IInternalAfterPipelineLogger : IAfterPipelineLogger
{
    /// <summary>
    /// Writes all buffered log messages to the logger.
    /// </summary>
    void WriteLogs();
}
