using ModularPipelines.Models;
using ModularPipelines.Reporting;

namespace ModularPipelines.Reporting;

/// <summary>
/// Convenience operations over <see cref="IRunHistoryStore"/>.
/// </summary>
public static class RunHistoryStoreExtensions
{
    /// <summary>
    /// Gets the latest retained report for a pipeline identity.
    /// </summary>
    /// <param name="historyStore">The history store.</param>
    /// <param name="pipelineIdentity">The stable pipeline identity.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>The latest matching report, or <see langword="null"/>.</returns>
    public static async Task<PipelineRunReport?> GetLatestAsync(
        this IRunHistoryStore historyStore,
        string pipelineIdentity,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(historyStore);

        await foreach (var report in historyStore.GetRunsAsync(
                               new RunHistoryQuery
                               {
                                   PipelineIdentity = pipelineIdentity,
                                   MaxRuns = 1,
                               },
                               cancellationToken)
                           .ConfigureAwait(false))
        {
            return report;
        }

        return null;
    }
}
