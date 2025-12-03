using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("peering", "registered-asn", "show")]
public record AzPeeringRegisteredAsnShowOptions(
[property: CliOption("--peering-name")] string PeeringName,
[property: CliOption("--registered-asn-name")] string RegisteredAsnName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;