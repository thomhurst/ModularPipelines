namespace ModularPipelines.Logging;

/// <summary>
/// Internal interface for summary logger operations not exposed to consumers.
/// </summary>
internal interface IInternalSummaryLogger : ISummaryLogger
{
    /// <summary>
    /// Writes all buffered log messages to the logger.
    /// </summary>
    void WriteLogs();
}
