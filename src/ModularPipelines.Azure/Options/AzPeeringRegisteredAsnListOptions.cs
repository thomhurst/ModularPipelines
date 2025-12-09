using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("peering", "registered-asn", "list")]
public record AzPeeringRegisteredAsnListOptions(
[property: CliOption("--peering-name")] string PeeringName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;