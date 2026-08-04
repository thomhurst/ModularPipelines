using ModularPipelines.Models;

namespace ModularPipelines.Engine;

/// <summary>
/// Stores bounded pipeline run history for cross-run comparisons.
/// </summary>
public interface IRunHistoryStore
{
    /// <summary>
    /// Gets retained reports matching a query, ordered newest first.
    /// </summary>
    /// <param name="query">The history query.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>The matching retained reports.</returns>
    IAsyncEnumerable<PipelineRunReport> GetRunsAsync(
        RunHistoryQuery query,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves a run report and applies the store's configured retention policy.
    /// </summary>
    /// <param name="report">The report to save.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>A task representing the save operation.</returns>
    Task SaveAsync(PipelineRunReport report, CancellationToken cancellationToken = default);
}
