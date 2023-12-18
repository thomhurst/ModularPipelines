using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vpn-connection", "create")]
public record AzNetworkVpnConnectionCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vnet-gateway1")] string VnetGateway1
) : AzOptions
{
    [CommandSwitch("--authorization-key")]
    public string? AuthorizationKey { get; set; }

    [CommandSwitch("--egress-nat-rule")]
    public string? EgressNatRule { get; set; }

    [BooleanCommandSwitch("--enable-bgp")]
    public bool? EnableBgp { get; set; }

    [CommandSwitch("--express-route-circuit2")]
    public string? ExpressRouteCircuit2 { get; set; }

    [BooleanCommandSwitch("--express-route-gateway-bypass")]
    public bool? ExpressRouteGatewayBypass { get; set; }

    [CommandSwitch("--ingress-nat-rule")]
    public string? IngressNatRule { get; set; }

    [CommandSwitch("--local-gateway2")]
    public string? LocalGateway2 { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--routing-weight")]
    public string? RoutingWeight { get; set; }

    [CommandSwitch("--shared-key")]
    public string? SharedKey { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [BooleanCommandSwitch("--use-policy-based-traffic-selectors")]
    public bool? UsePolicyBasedTrafficSelectors { get; set; }

    [BooleanCommandSwitch("--validate")]
    public bool? Validate { get; set; }

    [CommandSwitch("--vnet-gateway2")]
    public string? VnetGateway2 { get; set; }
}