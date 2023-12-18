using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "check-name-availability")]
public record AzNetworkFrontDoorCheckNameAvailabilityOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-type")] string ResourceType
) : AzOptions;