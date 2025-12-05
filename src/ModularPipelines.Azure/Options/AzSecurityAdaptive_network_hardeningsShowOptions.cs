using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "adaptive_network_hardenings", "show")]
public record AzSecurityAdaptive_network_hardeningsShowOptions(
[property: CliOption("--adaptive-network-hardenings-resource-name")] string AdaptiveNetworkHardeningsResourceName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-name")] string ResourceName,
[property: CliOption("--resource-namespace")] string ResourceNamespace,
[property: CliOption("--resource-type")] string ResourceType
) : AzOptions;