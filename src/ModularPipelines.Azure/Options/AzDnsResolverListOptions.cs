using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns-resolver", "list")]
public record AzDnsResolverListOptions : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }

    [CliOption("--virtual-network-name")]
    public string? VirtualNetworkName { get; set; }
}