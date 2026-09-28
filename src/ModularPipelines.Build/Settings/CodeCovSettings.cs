using ModularPipelines.Attributes;
using ModularPipelines.Secrets;

namespace ModularPipelines.Build.Settings;

public record CodeCovSettings
{
    [SecretValue]
    public string? Token { get; init; }
}
