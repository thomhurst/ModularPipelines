namespace ModularPipelines.Git.Models;

public record GitCommit
{
    public GitHash? Hash { get; set; }

    public GitAuthor? Author { get; set; }

    public GitAuthor? Committer { get; set; }

    public GitMessage? Message { get; set; }
}
