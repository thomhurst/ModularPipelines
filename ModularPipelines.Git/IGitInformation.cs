namespace ModularPipelines.Git;

public interface IGitInformation
{
    public string? BranchName { get; }
    public string? DefaultBranchName { get; }
    public string? LastCommitSha { get; }
    public string? LastCommitShortSha { get; }

    public string? Tag { get; }
    public string? LastCommit { get; }
    public Task<IEnumerable<string>> LastCommits(int count);
    public int CommitsOnBranch { get; }
    public DateTimeOffset LastCommitDateTime { get; }
}

public interface IGitInformation<T> : IGitInformation
{
}