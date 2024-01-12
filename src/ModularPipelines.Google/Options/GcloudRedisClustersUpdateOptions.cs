using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redis", "clusters", "update")]
public record GcloudRedisClustersUpdateOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--replica-count")]
    public string? ReplicaCount { get; set; }

    [CommandSwitch("--shard-count")]
    public string? ShardCount { get; set; }
}