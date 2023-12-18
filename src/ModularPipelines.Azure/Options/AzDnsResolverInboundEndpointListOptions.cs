using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns-resolver", "inbound-endpoint", "list")]
public record AzDnsResolverInboundEndpointListOptions(
[property: CommandSwitch("--dns-resolver-name")] string DnsResolverName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--top")]
    public string? Top { get; set; }
}

