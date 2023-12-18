using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "workload-network", "dns-service", "create")]
public record AzVmwareWorkloadNetworkDnsServiceCreateOptions(
[property: CommandSwitch("--dns-service")] string DnsService,
[property: CommandSwitch("--private-cloud")] string PrivateCloud,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--default-dns-zone")]
    public string? DefaultDnsZone { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--dns-service-ip")]
    public string? DnsServiceIp { get; set; }

    [CommandSwitch("--fqdn-zones")]
    public string? FqdnZones { get; set; }

    [CommandSwitch("--log-level")]
    public string? LogLevel { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--revision")]
    public string? Revision { get; set; }
}