using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmware", "workload-network", "dns-zone", "update")]
public record AzVmwareWorkloadNetworkDnsZoneUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--dns-server-ips")]
    public string? DnsServerIps { get; set; }

    [CliOption("--dns-services")]
    public string? DnsServices { get; set; }

    [CliOption("--dns-zone")]
    public string? DnsZone { get; set; }

    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

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

    [CliOption("--source-ip")]
    public string? SourceIp { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}