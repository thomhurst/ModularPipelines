namespace ModularPipelines.Logging;

/// <summary>
/// Reads entries buffered for the pipeline summary.
/// </summary>
public interface ISummaryLogReader
{
    /// <summary>
    /// Gets all recorded summary entries.
    /// </summary>
    IReadOnlyList<SummaryLogEntry> GetEntries();

    /// <summary>
    /// Gets recorded summary entries in a category.
    /// </summary>
    /// <param name="category">The category to filter by.</param>
    IReadOnlyList<SummaryLogEntry> GetEntries(string category);

    /// <summary>
    /// Gets all buffered summary messages as formatted text.
    /// </summary>
    string GetOutput();
}
