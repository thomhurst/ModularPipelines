namespace ModularPipelines.Logging;

/// <summary>
/// Internal interface for summary logger operations not exposed to consumers.
/// </summary>
internal interface IInternalSummaryLogger : ISummaryLogger, ISummaryLogReader
{
    /// <summary>
    /// Returns every buffered entry and marks them as displayed, so that
    /// <see cref="WriteLogs"/> does not log them a second time.
    /// </summary>
    /// <returns>The buffered entries in the order they were added.</returns>
    IReadOnlyList<SummaryLogEntry> TakeEntriesForDisplay();

    /// <summary>
    /// Writes buffered log messages that were not already displayed to the logger.
    /// </summary>
    void WriteLogs();
}
