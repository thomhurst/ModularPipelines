using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("peering", "peering", "show")]
public record AzPeeringPeeringShowOptions(
[property: CommandSwitch("--peering-name")] string PeeringName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;