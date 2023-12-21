using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vpn-gateway", "connection", "vpn-site-link-conn", "remove")]
public record AzNetworkVpnGatewayConnectionVpnSiteLinkConnRemoveOptions(
[property: CommandSwitch("--index")] string Index
) : AzOptions
{
    [CommandSwitch("--connection-name")]
    public string? ConnectionName { get; set; }

    [CommandSwitch("--gateway-name")]
    public string? GatewayName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}