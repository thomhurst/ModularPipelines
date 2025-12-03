using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("peering", "peering", "create")]
public record AzPeeringPeeringCreateOptions(
[property: CliOption("--kind")] string Kind,
[property: CliOption("--location")] string Location,
[property: CliOption("--peering-name")] string PeeringName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku
) : AzOptions
{
    [CliOption("--direct")]
    public string? Direct { get; set; }

    [CliOption("--exchange")]
    public string? Exchange { get; set; }

    [CliOption("--peering-location")]
    public string? PeeringLocation { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}