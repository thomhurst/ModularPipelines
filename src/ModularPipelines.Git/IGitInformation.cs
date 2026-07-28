using ModularPipelines.Git.Models;
using ModularPipelines.Git.Options;

namespace ModularPipelines.Git;

public interface IGitInformation
{
    Task<GitRepositoryInfo?> GetInfoAsync();

    IAsyncEnumerable<GitCommit> Commits(
        GitOptions? options = null,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GitCommit> Commits(
        string? branch,
        GitOptions? options = null,
        CancellationToken cancellationToken = default);
}
