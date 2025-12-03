using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vpn-connection", "update")]
public record AzNetworkVpnConnectionUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--enable-bgp")]
    public bool? EnableBgp { get; set; }

    [CliFlag("--express-route-gateway-bypass")]
    public bool? ExpressRouteGatewayBypass { get; set; }

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

    [CliOption("--routing-weight")]
    public string? RoutingWeight { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--shared-key")]
    public string? SharedKey { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--use-policy-based-traffic-selectors")]
    public bool? UsePolicyBasedTrafficSelectors { get; set; }
}