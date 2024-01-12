using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redis", "clusters", "create")]
public record GcloudRedisClustersCreateOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--network")] string Network,
[property: CommandSwitch("--shard-count")] string ShardCount
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [CommandSwitch("--replica-count")]
    public string? ReplicaCount { get; set; }

    [CommandSwitch("--transit-encryption-mode")]
    public string? TransitEncryptionMode { get; set; }
}