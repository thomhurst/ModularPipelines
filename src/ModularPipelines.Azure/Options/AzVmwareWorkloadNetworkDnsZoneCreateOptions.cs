using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "workload-network", "dns-zone", "create")]
public record AzVmwareWorkloadNetworkDnsZoneCreateOptions(
[property: CommandSwitch("--dns-zone")] string DnsZone,
[property: CommandSwitch("--private-cloud")] string PrivateCloud,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--dns-server-ips")]
    public string? DnsServerIps { get; set; }

    [CommandSwitch("--dns-services")]
    public string? DnsServices { get; set; }

    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--revision")]
    public string? Revision { get; set; }

    [CommandSwitch("--source-ip")]
    public string? SourceIp { get; set; }
}