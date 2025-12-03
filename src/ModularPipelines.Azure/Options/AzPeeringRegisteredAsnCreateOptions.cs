using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("peering", "registered-asn", "create")]
public record AzPeeringRegisteredAsnCreateOptions(
[property: CliOption("--peering-name")] string PeeringName,
[property: CliOption("--registered-asn-name")] string RegisteredAsnName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--asn")]
    public string? Asn { get; set; }
}