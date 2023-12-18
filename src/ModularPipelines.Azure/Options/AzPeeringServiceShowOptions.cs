using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("peering", "service", "show")]
public record AzPeeringServiceShowOptions(
[property: CommandSwitch("--peering-service-name")] string PeeringServiceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;