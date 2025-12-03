using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vpn-connection", "create")]
public record AzNetworkVpnConnectionCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vnet-gateway1")] string VnetGateway1
) : AzOptions
{
    [CliOption("--authorization-key")]
    public string? AuthorizationKey { get; set; }

    [CliOption("--egress-nat-rule")]
    public string? EgressNatRule { get; set; }

    [CliFlag("--enable-bgp")]
    public bool? EnableBgp { get; set; }

    [CliOption("--express-route-circuit2")]
    public string? ExpressRouteCircuit2 { get; set; }

    [CliFlag("--express-route-gateway-bypass")]
    public bool? ExpressRouteGatewayBypass { get; set; }

    [CliOption("--ingress-nat-rule")]
    public string? IngressNatRule { get; set; }

    [CliOption("--local-gateway2")]
    public string? LocalGateway2 { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--routing-weight")]
    public string? RoutingWeight { get; set; }

    [CliOption("--shared-key")]
    public string? SharedKey { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--use-policy-based-traffic-selectors")]
    public bool? UsePolicyBasedTrafficSelectors { get; set; }

    [CliFlag("--validate")]
    public bool? Validate { get; set; }

    [CliOption("--vnet-gateway2")]
    public string? VnetGateway2 { get; set; }
}