namespace ModularPipelines.Git;

internal class GitInformation<T> : IGitInformation<T>
{
    private readonly IGitInformation<StaticGitInformation> _staticGitInformation;

    public GitInformation(IGitInformation<StaticGitInformation> staticGitInformation)
    {
        _staticGitInformation = staticGitInformation;
    }

    public string BranchName => _staticGitInformation.BranchName;
    public string DefaultBranchName => _staticGitInformation.DefaultBranchName;

    public string Tag => _staticGitInformation.Tag;

    public string LastCommit => _staticGitInformation.LastCommit;

    public Task<IEnumerable<string>> LastCommits(int count) => _staticGitInformation.LastCommits(count);

    public int CommitsOnBranch => _staticGitInformation.CommitsOnBranch;
    public DateTimeOffset LastCommitDateTime => _staticGitInformation.LastCommitDateTime;
        
    public string LastCommitSha => _staticGitInformation.LastCommitSha;

    public string LastCommitShortSha => _staticGitInformation.LastCommitShortSha;
}