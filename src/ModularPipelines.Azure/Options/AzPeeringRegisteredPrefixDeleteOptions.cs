using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("peering", "registered-prefix", "delete")]
public record AzPeeringRegisteredPrefixDeleteOptions(
[property: CommandSwitch("--peering-name")] string PeeringName,
[property: CommandSwitch("--registered-prefix-name")] string RegisteredPrefixName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;