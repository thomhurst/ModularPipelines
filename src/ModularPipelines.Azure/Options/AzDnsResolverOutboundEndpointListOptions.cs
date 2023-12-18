using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns-resolver", "outbound-endpoint", "list")]
public record AzDnsResolverOutboundEndpointListOptions(
[property: CommandSwitch("--dns-resolver-name")] string DnsResolverName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--top")]
    public string? Top { get; set; }
}