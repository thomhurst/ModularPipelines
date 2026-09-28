using ModularPipelines.Attributes;
using ModularPipelines.Secrets;

namespace ModularPipelines.Build.Settings;

public record CodacySettings
{
    [SecretValue]
    public string? ApiKey { get; set; }
}