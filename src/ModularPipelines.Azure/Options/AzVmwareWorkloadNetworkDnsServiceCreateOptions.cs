using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "workload-network", "dns-service", "create")]
public record AzVmwareWorkloadNetworkDnsServiceCreateOptions(
[property: CliOption("--dns-service")] string DnsService,
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--default-dns-zone")]
    public string? DefaultDnsZone { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--dns-service-ip")]
    public string? DnsServiceIp { get; set; }

    [CliOption("--fqdn-zones")]
    public string? FqdnZones { get; set; }

    [CliOption("--log-level")]
    public string? LogLevel { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--revision")]
    public string? Revision { get; set; }
}