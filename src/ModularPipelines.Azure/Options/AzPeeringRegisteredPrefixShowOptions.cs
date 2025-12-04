using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("peering", "registered-prefix", "show")]
public record AzPeeringRegisteredPrefixShowOptions(
[property: CliOption("--peering-name")] string PeeringName,
[property: CliOption("--registered-prefix-name")] string RegisteredPrefixName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;