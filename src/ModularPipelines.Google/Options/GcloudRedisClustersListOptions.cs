using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "clusters", "list")]
public record GcloudRedisClustersListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}