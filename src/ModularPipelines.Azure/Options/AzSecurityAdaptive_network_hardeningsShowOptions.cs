using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "adaptive_network_hardenings", "show")]
public record AzSecurityAdaptive_network_hardeningsShowOptions(
[property: CommandSwitch("--adaptive-network-hardenings-resource-name")] string AdaptiveNetworkHardeningsResourceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--resource-namespace")] string ResourceNamespace,
[property: CommandSwitch("--resource-type")] string ResourceType
) : AzOptions;