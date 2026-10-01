using ModularPipelines.Git.Models;
using ModularPipelines.Git.Options;

namespace ModularPipelines.Git;

public interface IGitInformation
{
    /// <summary>
    /// Gets cached information about the current Git repository.
    /// </summary>
    /// <param name="cancellationToken">The token used to cancel repository discovery.</param>
    /// <returns>The repository information, or <see langword="null" /> when Git information is unavailable.</returns>
    Task<GitRepositoryInfo?> GetInfoAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Enumerates commits from the current branch.
    /// </summary>
    /// <remarks>
    /// Each enumeration runs in its own service scope, so it is safe to call from singleton services.
    /// <see cref="IGitHistoryCommands.CommitsAsync(GitOptions?, CancellationToken)"/> returns the same commits through the command group.
    /// </remarks>
    IAsyncEnumerable<GitCommit> CommitsAsync(
        GitOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Enumerates commits from the specified branch.
    /// </summary>
    /// <remarks>
    /// Each enumeration runs in its own service scope, so it is safe to call from singleton services.
    /// <see cref="IGitHistoryCommands.CommitsAsync(string?, GitOptions?, CancellationToken)"/> returns the same commits through the command group.
    /// </remarks>
    IAsyncEnumerable<GitCommit> CommitsAsync(
        string? branch,
        GitOptions? options = null,
        CancellationToken cancellationToken = default);
}
