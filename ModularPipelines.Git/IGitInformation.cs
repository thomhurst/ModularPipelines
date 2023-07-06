using ModularPipelines.FileSystem;
using ModularPipelines.Git.Models;
using ModularPipelines.Git.Options;

namespace ModularPipelines.Git;

public interface IGitInformation
{
    public Folder Root { get; }
    public string? BranchName { get; }
    public string? DefaultBranchName { get; }
    public string? LastCommitSha { get; }
    public string? LastCommitShortSha { get; }

    public string? Tag { get; }
    public int CommitsOnBranch { get; }
    public DateTimeOffset LastCommitDateTime { get; }
    public GitCommit? PreviousCommit { get; }
    IAsyncEnumerable<GitCommit> Commits(GitOptions? options = null, CancellationToken cancellationToken = default);
    IAsyncEnumerable<GitCommit> Commits(string branch, GitOptions? options = null, CancellationToken cancellationToken = default);
}
