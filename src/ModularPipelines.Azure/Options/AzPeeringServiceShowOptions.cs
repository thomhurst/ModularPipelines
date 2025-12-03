using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("peering", "service", "show")]
public record AzPeeringServiceShowOptions(
[property: CliOption("--peering-service-name")] string PeeringServiceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;