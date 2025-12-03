using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "express-route", "gateway", "connection", "update")]
public record AzNetworkExpressRouteGatewayConnectionUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--associated")]
    public string? Associated { get; set; }

    [CliOption("--authorization-key")]
    public string? AuthorizationKey { get; set; }

    [CliOption("--circuit-name")]
    public string? CircuitName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--gateway-name")]
    public string? GatewayName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--inbound-route-map")]
    public string? InboundRouteMap { get; set; }

    [CliFlag("--internet-security")]
    public bool? InternetSecurity { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--outbound-route-map")]
    public string? OutboundRouteMap { get; set; }

    [CliOption("--peering")]
    public string? Peering { get; set; }

    [CliOption("--propagated")]
    public string? Propagated { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--routing-weight")]
    public string? RoutingWeight { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}