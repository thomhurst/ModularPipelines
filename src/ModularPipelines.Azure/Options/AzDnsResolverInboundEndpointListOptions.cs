using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

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