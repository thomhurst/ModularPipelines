using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("peering", "legacy", "list")]
public record AzPeeringLegacyListOptions(
[property: CliOption("--kind")] string Kind,
[property: CliOption("--peering-location")] string PeeringLocation
) : AzOptions
{
    [CliOption("--asn")]
    public string? Asn { get; set; }
}