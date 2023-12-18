using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "managed-cluster", "update")]
public record AzSfManagedClusterUpdateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--client-connection-port")]
    public string? ClientConnectionPort { get; set; }

    [CommandSwitch("--dns-name")]
    public string? DnsName { get; set; }

    [CommandSwitch("--gateway-connection-port")]
    public string? GatewayConnectionPort { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}