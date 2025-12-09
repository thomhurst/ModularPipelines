using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("peering", "registered-prefix", "list")]
public record AzPeeringRegisteredPrefixListOptions(
[property: CliOption("--peering-name")] string PeeringName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;