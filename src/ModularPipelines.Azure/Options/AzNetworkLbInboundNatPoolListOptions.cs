using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "lb", "inbound-nat-pool", "list")]
public record AzNetworkLbInboundNatPoolListOptions(
[property: CommandSwitch("--lb-name")] string LbName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;