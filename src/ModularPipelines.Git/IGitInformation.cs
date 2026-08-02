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
    /// Async-enumerable methods omit the <c>Async</c> suffix because enumeration is asynchronous at the call site.
    /// </remarks>
    IAsyncEnumerable<GitCommit> Commits(
        GitOptions? options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Enumerates commits from the specified branch.
    /// </summary>
    /// <remarks>
    /// Async-enumerable methods omit the <c>Async</c> suffix because enumeration is asynchronous at the call site.
    /// </remarks>
    IAsyncEnumerable<GitCommit> Commits(
        string? branch,
        GitOptions? options = null,
        CancellationToken cancellationToken = default);
}
