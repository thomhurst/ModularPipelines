using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "zones", "list")]
public record GcloudRedisZonesListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}