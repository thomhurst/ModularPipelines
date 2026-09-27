namespace ModularPipelines.Logging;

/// <summary>
/// Internal interface for summary logger operations not exposed to consumers.
/// </summary>
internal interface IInternalSummaryLogger : ISummaryLogger, ISummaryLogReader
{
    /// <summary>
    /// Returns every buffered entry for display in the results output.
    /// </summary>
    /// <returns>The buffered entries in the order they were added.</returns>
    IReadOnlyList<SummaryLogEntry> GetEntriesForDisplay();

    /// <summary>
    /// Records that the first <paramref name="count"/> entries were rendered in the results
    /// output, so <see cref="WriteLogs"/> keeps them off the console.
    /// </summary>
    /// <param name="count">The number of entries, from the start, that were displayed.</param>
    void MarkDisplayed(int count);

    /// <summary>
    /// Writes buffered entries to the logging providers. Entries already displayed in the results
    /// output are sent to every provider except the console.
    /// </summary>
    void WriteLogs();
}
