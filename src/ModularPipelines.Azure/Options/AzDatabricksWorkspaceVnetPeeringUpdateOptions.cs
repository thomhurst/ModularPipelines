using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databricks", "workspace", "vnet-peering", "update")]
public record AzDatabricksWorkspaceVnetPeeringUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [BooleanCommandSwitch("--allow-forwarded-traffic")]
    public bool? AllowForwardedTraffic { get; set; }

    [BooleanCommandSwitch("--allow-gateway-transit")]
    public bool? AllowGatewayTransit { get; set; }

    [BooleanCommandSwitch("--allow-virtual-network-access")]
    public bool? AllowVirtualNetworkAccess { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [BooleanCommandSwitch("--use-remote-gateways")]
    public bool? UseRemoteGateways { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}