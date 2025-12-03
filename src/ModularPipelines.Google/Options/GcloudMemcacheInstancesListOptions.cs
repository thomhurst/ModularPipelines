using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memcache", "instances", "list")]
public record GcloudMemcacheInstancesListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}