using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "instances", "list")]
public record GcloudRedisInstancesListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}