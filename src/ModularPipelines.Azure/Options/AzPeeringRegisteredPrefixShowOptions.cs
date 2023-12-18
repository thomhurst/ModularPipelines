using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("peering", "registered-prefix", "show")]
public record AzPeeringRegisteredPrefixShowOptions(
[property: CommandSwitch("--peering-name")] string PeeringName,
[property: CommandSwitch("--registered-prefix-name")] string RegisteredPrefixName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}