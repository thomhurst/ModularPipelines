using ModularPipelines.FileSystem;
using ModularPipelines.Git.Models;

namespace ModularPipelines.Git;

public interface IGitInformation
{
    Folder Root { get; }
    string BranchName { get; }
    string DefaultBranchName { get; }
    string LastCommitSha { get; }
    string LastCommitShortSha { get; }
    string Tag { get; }
    int CommitsOnBranch { get; }
    DateTimeOffset LastCommitDateTime { get; }
    GitCommit? PreviousCommit { get; }
}
