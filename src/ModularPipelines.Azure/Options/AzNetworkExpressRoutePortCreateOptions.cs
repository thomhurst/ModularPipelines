using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "express-route", "port", "create")]
public record AzNetworkExpressRoutePortCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--bandwidth")]
    public string? Bandwidth { get; set; }

    [CommandSwitch("--encapsulation")]
    public string? Encapsulation { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--peering-location")]
    public string? PeeringLocation { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

