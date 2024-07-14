using ModularPipelines.Attributes;

namespace ModularPipelines.GitHub.Options;

public record GitHubOptions
{
  [SecretValue]
  public string? AccessToken { get; set; }
}