using ModularPipelines.Models;

namespace ModularPipelines.Engine;

/// <summary>
/// Provides convenient cross-run history queries for the current pipeline.
/// </summary>
public interface IRunHistoryReader
{
    /// <summary>
    /// Gets measured duration samples for a module over the latest pipeline runs.
    /// </summary>
    /// <param name="moduleTypeName">The stable module type identifier.</param>
    /// <param name="lastN">The number of latest pipeline runs to inspect.</param>
    /// <param name="cancellationToken">A token that cancels the operation.</param>
    /// <returns>Newest-first attributable module duration samples.</returns>
    Task<IReadOnlyList<ModuleDurationSample>> GetModuleDurationTrendAsync(
        string moduleTypeName,
        int lastN,
        CancellationToken cancellationToken = default);
}
