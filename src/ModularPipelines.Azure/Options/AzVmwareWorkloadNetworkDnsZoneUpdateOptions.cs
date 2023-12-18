using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "workload-network", "dns-zone", "update")]
public record AzVmwareWorkloadNetworkDnsZoneUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--dns-server-ips")]
    public string? DnsServerIps { get; set; }

    [CommandSwitch("--dns-services")]
    public string? DnsServices { get; set; }

    [CommandSwitch("--dns-zone")]
    public string? DnsZone { get; set; }

    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--private-cloud")]
    public string? PrivateCloud { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--revision")]
    public string? Revision { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--source-ip")]
    public string? SourceIp { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}