namespace ModularPipelines.Git.Models;

public record GitCommit
{
    public GitHash? Hash { get; init; }

    public GitAuthor? Author { get; init; }

    public GitAuthor? Committer { get; init; }

    public GitMessage? Message { get; init; }
}