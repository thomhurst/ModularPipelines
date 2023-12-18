using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "express-route", "gateway", "connection", "update")]
public record AzNetworkExpressRouteGatewayConnectionUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--associated")]
    public string? Associated { get; set; }

    [CommandSwitch("--authorization-key")]
    public string? AuthorizationKey { get; set; }

    [CommandSwitch("--circuit-name")]
    public string? CircuitName { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--gateway-name")]
    public string? GatewayName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--inbound-route-map")]
    public string? InboundRouteMap { get; set; }

    [BooleanCommandSwitch("--internet-security")]
    public bool? InternetSecurity { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--outbound-route-map")]
    public string? OutboundRouteMap { get; set; }

    [CommandSwitch("--peering")]
    public string? Peering { get; set; }

    [CommandSwitch("--propagated")]
    public string? Propagated { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--routing-weight")]
    public string? RoutingWeight { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

