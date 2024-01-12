using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redis", "zones", "list")]
public record GcloudRedisZonesListOptions : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}