using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "lb", "frontend-ip", "list")]
public record AzNetworkLbFrontendIpListOptions(
[property: CommandSwitch("--lb-name")] string LbName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;