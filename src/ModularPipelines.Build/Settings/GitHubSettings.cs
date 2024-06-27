using ModularPipelines.Attributes;

namespace ModularPipelines.Build.Settings;

public record GitHubSettings
{
    [SecretValue]
    public string? StandardToken { get; init; }

    [SecretValue]
    public string? AdminToken { get; init; }
}