using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("peering", "service", "delete")]
public record AzPeeringServiceDeleteOptions(
[property: CliOption("--peering-service-name")] string PeeringServiceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;