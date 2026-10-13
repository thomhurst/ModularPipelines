using ModularPipelines.Attributes;
using ModularPipelines.Secrets;

namespace ModularPipelines.GitHub;

public record GitHubOptions
{
    [SecretValue]
    public string? AccessToken { get; set; }
}