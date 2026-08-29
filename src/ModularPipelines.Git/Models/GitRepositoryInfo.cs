using ModularPipelines.FileSystem;

namespace ModularPipelines.Git.Models;

public sealed record GitRepositoryInfo(FolderPath Root)
{
    public string? BranchName { get; init; }

    public string? DefaultBranchName { get; init; }

    public string? LastCommitSha { get; init; }

    public string? LastCommitShortSha { get; init; }

    public string? Tag { get; init; }

    public int? CommitsOnBranch { get; init; }

    public DateTimeOffset? LastCommitDateTime { get; init; }

    public GitCommit? PreviousCommit { get; init; }
}
