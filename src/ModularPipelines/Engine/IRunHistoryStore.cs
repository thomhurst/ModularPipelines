using ModularPipelines.Models;

namespace ModularPipelines.Engine;

/// <summary>
/// Stores bounded pipeline run history for cross-run comparisons.
/// </summary>
public interface IRunHistoryStore
{
    /// <summary>
    /// Gets the latest retained report for a pipeline identity.
    /// </summary>
    /// <param name="pipelineIdentity">The stable pipeline identity.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>The latest matching report, or <see langword="null"/>.</returns>
    Task<PipelineRunReport?> GetLatestAsync(
        string pipelineIdentity,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves a run report and applies the store's configured retention policy.
    /// </summary>
    /// <param name="report">The report to save.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>A task representing the save operation.</returns>
    Task SaveAsync(PipelineRunReport report, CancellationToken cancellationToken = default);
}
