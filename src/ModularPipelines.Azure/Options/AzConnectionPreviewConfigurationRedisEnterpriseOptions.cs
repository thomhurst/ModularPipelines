using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connection", "preview-configuration", "redis-enterprise")]
public record AzConnectionPreviewConfigurationRedisEnterpriseOptions : AzOptions
{
    [CommandSwitch("--client-type")]
    public string? ClientType { get; set; }

    [CommandSwitch("--secret")]
    public string? Secret { get; set; }
}