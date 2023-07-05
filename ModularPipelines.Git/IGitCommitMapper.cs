using ModularPipelines.Git.Models;

namespace ModularPipelines.Git;

internal interface IGitCommitMapper
{
    GitCommit Map(string commandLineOutput);
}
