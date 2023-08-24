namespace ModularPipelines.Build.Settings;

public record GitHubSettings
{
    public string? TokenWithTriggerBuild { get; init; }
    public string? TokenWithoutTriggerBuild { get; init; }
    public GitHubPullRequest? PullRequest { get; init; }
    public GitHubRepository? Repository { get; init; }
}

public record GitHubRepository
{
    public long? Id { get; init; }
}

public record GitHubPullRequest
{
    public int? Number { get; init; }
    public string? Branch { get; init; }
}