using ModularPipelines.Attributes;

namespace ModularPipelines.Build.Settings;

public record CodacySettings
{
    [SecretValue]
    public string? ApiKey { get; set; }
}