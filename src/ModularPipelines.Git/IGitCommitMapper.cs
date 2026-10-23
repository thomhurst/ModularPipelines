namespace ModularPipelines.Git;

public interface IGitCommitMapper
{
    GitCommit Map(string commandLineOutput);
}
