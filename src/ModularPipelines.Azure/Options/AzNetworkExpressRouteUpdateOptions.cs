using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "express-route", "update")]
public record AzNetworkExpressRouteUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--allow-classic-operations")]
    public bool? AllowClassicOperations { get; set; }

    [CliFlag("--allow-global-reach")]
    public bool? AllowGlobalReach { get; set; }

    [CliOption("--bandwidth")]
    public string? Bandwidth { get; set; }

    [CliOption("--express-route-port")]
    public string? ExpressRoutePort { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--peering-location")]
    public string? PeeringLocation { get; set; }

    [CliOption("--provider")]
    public string? Provider { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--sku-family")]
    public string? SkuFamily { get; set; }

    [CliOption("--sku-tier")]
    public string? SkuTier { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}