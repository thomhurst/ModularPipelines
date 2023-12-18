using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redisenterprise", "database", "create")]
public record AzRedisenterpriseDatabaseCreateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--client-protocol")]
    public string? ClientProtocol { get; set; }

    [CommandSwitch("--clustering-policy")]
    public string? ClusteringPolicy { get; set; }

    [CommandSwitch("--eviction-policy")]
    public string? EvictionPolicy { get; set; }

    [CommandSwitch("--group-nickname")]
    public string? GroupNickname { get; set; }

    [CommandSwitch("--linked-database")]
    public string? LinkedDatabase { get; set; }

    [CommandSwitch("--mods")]
    public string? Mods { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--persistence")]
    public string? Persistence { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }
}