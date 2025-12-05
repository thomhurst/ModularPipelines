using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dns-resolver", "inbound-endpoint", "show")]
public record AzDnsResolverInboundEndpointShowOptions : AzOptions
{
    [CliOption("--dns-resolver-name")]
    public string? DnsResolverName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--inbound-endpoint-name")]
    public string? InboundEndpointName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}