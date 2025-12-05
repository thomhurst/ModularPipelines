using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "clusters", "delete")]
public record GcloudRedisClustersDeleteOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}