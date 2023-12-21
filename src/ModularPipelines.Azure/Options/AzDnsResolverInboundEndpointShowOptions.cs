using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns-resolver", "inbound-endpoint", "show")]
public record AzDnsResolverInboundEndpointShowOptions : AzOptions
{
    [CommandSwitch("--dns-resolver-name")]
    public string? DnsResolverName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--inbound-endpoint-name")]
    public string? InboundEndpointName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}