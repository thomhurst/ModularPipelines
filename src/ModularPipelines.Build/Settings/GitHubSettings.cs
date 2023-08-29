using ModularPipelines.Attributes;

namespace ModularPipelines.Build.Settings;

public record GitHubSettings
{
    [SecretValue]
    public string? StandardToken { get; init; }
    
    [SecretValue]
    public string? AdminToken { get; init; }
    
    public string? Actor { get; init; }
    
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
    public string? Author { get; init; }
}