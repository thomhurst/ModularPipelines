using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns-resolver", "forwarding-ruleset", "list-by-virtual-network")]
public record AzDnsResolverForwardingRulesetListByVirtualNetworkOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--virtual-network-name")] string VirtualNetworkName
) : AzOptions
{
    [CommandSwitch("--top")]
    public string? Top { get; set; }
}