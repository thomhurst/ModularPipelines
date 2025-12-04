using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmware", "workload-network", "dns-service", "update")]
public record AzVmwareWorkloadNetworkDnsServiceUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--default-dns-zone")]
    public string? DefaultDnsZone { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--dns-service")]
    public string? DnsService { get; set; }

    [CliOption("--dns-service-ip")]
    public string? DnsServiceIp { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--fqdn-zones")]
    public string? FqdnZones { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--log-level")]
    public string? LogLevel { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--private-cloud")]
    public string? PrivateCloud { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--revision")]
    public string? Revision { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}