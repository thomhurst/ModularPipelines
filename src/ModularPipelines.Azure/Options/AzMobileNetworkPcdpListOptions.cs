using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile-network", "pcdp", "list")]
public record AzMobileNetworkPcdpListOptions(
[property: CommandSwitch("--pccp-name")] string PccpName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;