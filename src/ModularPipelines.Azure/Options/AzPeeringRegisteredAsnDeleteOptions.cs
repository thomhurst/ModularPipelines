using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("peering", "registered-asn", "delete")]
public record AzPeeringRegisteredAsnDeleteOptions(
[property: CliOption("--peering-name")] string PeeringName,
[property: CliOption("--registered-asn-name")] string RegisteredAsnName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;