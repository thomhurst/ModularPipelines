using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns-resolver", "list-by-virtual-network")]
public record AzDnsResolverListByVirtualNetworkOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--virtual-network-name")] string VirtualNetworkName
) : AzOptions
{
    [CommandSwitch("--top")]
    public string? Top { get; set; }
}