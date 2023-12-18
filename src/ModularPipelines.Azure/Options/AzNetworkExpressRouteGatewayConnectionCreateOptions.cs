using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "express-route", "gateway", "connection", "create")]
public record AzNetworkExpressRouteGatewayConnectionCreateOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--associated")]
    public string? Associated { get; set; }

    [CommandSwitch("--authorization-key")]
    public string? AuthorizationKey { get; set; }

    [CommandSwitch("--circuit-name")]
    public string? CircuitName { get; set; }

    [CommandSwitch("--inbound-route-map")]
    public string? InboundRouteMap { get; set; }

    [BooleanCommandSwitch("--internet-security")]
    public bool? InternetSecurity { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--outbound-route-map")]
    public string? OutboundRouteMap { get; set; }

    [CommandSwitch("--peering")]
    public string? Peering { get; set; }

    [CommandSwitch("--propagated")]
    public string? Propagated { get; set; }

    [CommandSwitch("--routing-weight")]
    public string? RoutingWeight { get; set; }
}