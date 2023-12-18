using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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
    public string? Subscription { get; set; }
}