using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "check-name-availability")]
public record AzNetworkFrontDoorCheckNameAvailabilityOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-type")] string ResourceType
) : AzOptions;