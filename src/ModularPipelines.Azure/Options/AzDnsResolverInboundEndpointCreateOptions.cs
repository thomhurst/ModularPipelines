using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dns-resolver", "inbound-endpoint", "create")]
public record AzDnsResolverInboundEndpointCreateOptions(
[property: CliOption("--dns-resolver-name")] string DnsResolverName,
[property: CliOption("--inbound-endpoint-name")] string InboundEndpointName,
[property: CliOption("--ip-configurations")] string IpConfigurations,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}