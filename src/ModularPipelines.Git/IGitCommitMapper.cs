using ModularPipelines.Git.Models;

namespace ModularPipelines.Git;

public interface IGitCommitMapper
{
    GitCommit Map(string commandLineOutput);
}