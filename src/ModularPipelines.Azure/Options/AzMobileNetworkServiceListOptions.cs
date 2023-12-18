using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile-network", "service", "list")]
public record AzMobileNetworkServiceListOptions(
[property: CommandSwitch("--mobile-network-name")] string MobileNetworkName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;