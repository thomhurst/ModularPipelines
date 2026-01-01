using ModularPipelines.Attributes;

namespace ModularPipelines.Build.Settings;

public record CodeCovSettings
{
    [SecretValue]
    public string? Token { get; init; }
}
