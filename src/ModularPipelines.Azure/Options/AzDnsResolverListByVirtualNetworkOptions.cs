using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns-resolver", "list-by-virtual-network")]
public record AzDnsResolverListByVirtualNetworkOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--virtual-network-name")] string VirtualNetworkName
) : AzOptions
{
    [CliOption("--top")]
    public string? Top { get; set; }
}