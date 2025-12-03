using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "express-route", "gateway", "connection", "create")]
public record AzNetworkExpressRouteGatewayConnectionCreateOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--associated")]
    public string? Associated { get; set; }

    [CliOption("--authorization-key")]
    public string? AuthorizationKey { get; set; }

    [CliOption("--circuit-name")]
    public string? CircuitName { get; set; }

    [CliOption("--inbound-route-map")]
    public string? InboundRouteMap { get; set; }

    [CliFlag("--internet-security")]
    public bool? InternetSecurity { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--outbound-route-map")]
    public string? OutboundRouteMap { get; set; }

    [CliOption("--peering")]
    public string? Peering { get; set; }

    [CliOption("--propagated")]
    public string? Propagated { get; set; }

    [CliOption("--routing-weight")]
    public string? RoutingWeight { get; set; }
}