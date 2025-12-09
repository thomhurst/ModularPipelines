using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "express-route", "port", "create")]
public record AzNetworkExpressRoutePortCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--bandwidth")]
    public string? Bandwidth { get; set; }

    [CliOption("--encapsulation")]
    public string? Encapsulation { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--peering-location")]
    public string? PeeringLocation { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}