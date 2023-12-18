using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile-network", "site", "list")]
public record AzMobileNetworkSiteListOptions(
[property: CommandSwitch("--mobile-network-name")] string MobileNetworkName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;