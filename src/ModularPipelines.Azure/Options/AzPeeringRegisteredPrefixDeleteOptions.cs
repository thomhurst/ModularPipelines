using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("peering", "registered-prefix", "delete")]
public record AzPeeringRegisteredPrefixDeleteOptions(
[property: CliOption("--peering-name")] string PeeringName,
[property: CliOption("--registered-prefix-name")] string RegisteredPrefixName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;