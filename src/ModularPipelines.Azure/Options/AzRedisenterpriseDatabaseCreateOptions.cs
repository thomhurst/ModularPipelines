using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("redisenterprise", "database", "create")]
public record AzRedisenterpriseDatabaseCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--client-protocol")]
    public string? ClientProtocol { get; set; }

    [CliOption("--clustering-policy")]
    public string? ClusteringPolicy { get; set; }

    [CliOption("--eviction-policy")]
    public string? EvictionPolicy { get; set; }

    [CliOption("--group-nickname")]
    public string? GroupNickname { get; set; }

    [CliOption("--linked-database")]
    public string? LinkedDatabase { get; set; }

    [CliOption("--mods")]
    public string? Mods { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--persistence")]
    public string? Persistence { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }
}