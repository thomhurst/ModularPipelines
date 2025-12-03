using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vpn-gateway", "connection", "vpn-site-link-conn", "ipsec-policy", "remove")]
public record AzNetworkVpnGatewayConnectionVpnSiteLinkConnIpsecPolicyRemoveOptions(
[property: CliOption("--index")] string Index
) : AzOptions
{
    [CliOption("--connection-name")]
    public string? ConnectionName { get; set; }

    [CliOption("--gateway-name")]
    public string? GatewayName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}