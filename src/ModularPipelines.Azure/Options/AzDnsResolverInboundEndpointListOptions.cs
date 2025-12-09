using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dns-resolver", "inbound-endpoint", "list")]
public record AzDnsResolverInboundEndpointListOptions(
[property: CliOption("--dns-resolver-name")] string DnsResolverName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--top")]
    public string? Top { get; set; }
}