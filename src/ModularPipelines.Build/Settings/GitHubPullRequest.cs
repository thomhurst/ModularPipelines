namespace ModularPipelines.Build.Settings;

public record GitHubPullRequest
{
    public int? Number { get; init; }
    public string? Branch { get; init; }
    public string? Sha { get; init; }
    public string? Author { get; init; }
}