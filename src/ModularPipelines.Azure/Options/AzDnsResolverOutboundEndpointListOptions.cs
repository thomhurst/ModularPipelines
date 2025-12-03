using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns-resolver", "outbound-endpoint", "list")]
public record AzDnsResolverOutboundEndpointListOptions(
[property: CliOption("--dns-resolver-name")] string DnsResolverName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--top")]
    public string? Top { get; set; }
}