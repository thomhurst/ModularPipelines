using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "clusters", "update")]
public record GcloudRedisClustersUpdateOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--replica-count")]
    public string? ReplicaCount { get; set; }

    [CliOption("--shard-count")]
    public string? ShardCount { get; set; }
}