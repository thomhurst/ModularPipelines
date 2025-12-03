using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "express-route", "create")]
public record AzNetworkExpressRouteCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--allow-classic-operations")]
    public bool? AllowClassicOperations { get; set; }

    [CliFlag("--allow-global-reach")]
    public bool? AllowGlobalReach { get; set; }

    [CliOption("--bandwidth")]
    public string? Bandwidth { get; set; }

    [CliOption("--express-route-port")]
    public string? ExpressRoutePort { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--peering-location")]
    public string? PeeringLocation { get; set; }

    [CliOption("--provider")]
    public string? Provider { get; set; }

    [CliOption("--sku-family")]
    public string? SkuFamily { get; set; }

    [CliOption("--sku-tier")]
    public string? SkuTier { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}