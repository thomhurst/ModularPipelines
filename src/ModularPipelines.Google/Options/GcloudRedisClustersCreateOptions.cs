using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "clusters", "create")]
public record GcloudRedisClustersCreateOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Region,
[property: CliOption("--network")] string Network,
[property: CliOption("--shard-count")] string ShardCount
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--replica-count")]
    public string? ReplicaCount { get; set; }

    [CliOption("--transit-encryption-mode")]
    public string? TransitEncryptionMode { get; set; }
}