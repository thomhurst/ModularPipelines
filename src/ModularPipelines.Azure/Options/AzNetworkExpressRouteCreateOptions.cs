using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "express-route", "create")]
public record AzNetworkExpressRouteCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--allow-classic-operations")]
    public bool? AllowClassicOperations { get; set; }

    [BooleanCommandSwitch("--allow-global-reach")]
    public bool? AllowGlobalReach { get; set; }

    [CommandSwitch("--bandwidth")]
    public string? Bandwidth { get; set; }

    [CommandSwitch("--express-route-port")]
    public string? ExpressRoutePort { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--peering-location")]
    public string? PeeringLocation { get; set; }

    [CommandSwitch("--provider")]
    public string? Provider { get; set; }

    [CommandSwitch("--sku-family")]
    public string? SkuFamily { get; set; }

    [CommandSwitch("--sku-tier")]
    public string? SkuTier { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

