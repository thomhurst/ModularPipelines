using ModularPipelines.Attributes;

namespace ModularPipelines.Build.Settings;

public record NuGetSettings
{
    [SecretValue]
    public string? ApiKey { get; init; }
}
