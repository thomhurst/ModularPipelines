using ModularPipelines.Attributes;

namespace ModularPipelines.Build.Settings;

public record GitHubSettings
{
    [SecretValue]
    public string? StandardToken { get; init; }
    
    [SecretValue]
    public string? AdminToken { get; init; }
    
    public string? Actor { get; init; }
    
    public string? Ref { get; init; }
    public string? Sha { get; init; }
    
    public GitHubPullRequest? PullRequest { get; init; }
    
    public GitHubRepository? Repository { get; init; }
}