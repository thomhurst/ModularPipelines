using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("databricks", "workspace", "vnet-peering", "update")]
public record AzDatabricksWorkspaceVnetPeeringUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--allow-forwarded-traffic")]
    public bool? AllowForwardedTraffic { get; set; }

    [CliFlag("--allow-gateway-transit")]
    public bool? AllowGatewayTransit { get; set; }

    [CliFlag("--allow-virtual-network-access")]
    public bool? AllowVirtualNetworkAccess { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--use-remote-gateways")]
    public bool? UseRemoteGateways { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}