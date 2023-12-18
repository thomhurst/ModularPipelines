using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "adaptive_network_hardenings", "list")]
public record AzSecurityAdaptive_network_hardeningsListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--resource-namespace")] string ResourceNamespace,
[property: CommandSwitch("--resource-type")] string ResourceType
) : AzOptions;