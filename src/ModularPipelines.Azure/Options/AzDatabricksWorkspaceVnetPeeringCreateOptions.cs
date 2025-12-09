using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("databricks", "workspace", "vnet-peering", "create")]
public record AzDatabricksWorkspaceVnetPeeringCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliFlag("--allow-forwarded-traffic")]
    public bool? AllowForwardedTraffic { get; set; }

    [CliFlag("--allow-gateway-transit")]
    public bool? AllowGatewayTransit { get; set; }

    [CliFlag("--allow-virtual-network-access")]
    public bool? AllowVirtualNetworkAccess { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remote-vnet")]
    public string? RemoteVnet { get; set; }

    [CliFlag("--use-remote-gateways")]
    public bool? UseRemoteGateways { get; set; }
}