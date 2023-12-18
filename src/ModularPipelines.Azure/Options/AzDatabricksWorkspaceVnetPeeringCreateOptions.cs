using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databricks", "workspace", "vnet-peering", "create")]
public record AzDatabricksWorkspaceVnetPeeringCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [BooleanCommandSwitch("--allow-forwarded-traffic")]
    public bool? AllowForwardedTraffic { get; set; }

    [BooleanCommandSwitch("--allow-gateway-transit")]
    public bool? AllowGatewayTransit { get; set; }

    [BooleanCommandSwitch("--allow-virtual-network-access")]
    public bool? AllowVirtualNetworkAccess { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--remote-vnet")]
    public string? RemoteVnet { get; set; }

    [BooleanCommandSwitch("--use-remote-gateways")]
    public bool? UseRemoteGateways { get; set; }
}