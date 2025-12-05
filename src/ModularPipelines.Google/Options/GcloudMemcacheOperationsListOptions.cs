using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memcache", "operations", "list")]
public record GcloudMemcacheOperationsListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}