using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmware", "workload-network", "dns-zone", "create")]
public record AzVmwareWorkloadNetworkDnsZoneCreateOptions(
[property: CliOption("--dns-zone")] string DnsZone,
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--dns-server-ips")]
    public string? DnsServerIps { get; set; }

    [CliOption("--dns-services")]
    public string? DnsServices { get; set; }

    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--revision")]
    public string? Revision { get; set; }

    [CliOption("--source-ip")]
    public string? SourceIp { get; set; }
}