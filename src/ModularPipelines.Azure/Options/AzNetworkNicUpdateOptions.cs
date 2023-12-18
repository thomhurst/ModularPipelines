using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "nic", "update")]
public record AzNetworkNicUpdateOptions : AzOptions
{
    [BooleanCommandSwitch("--accelerated-networking")]
    public bool? AcceleratedNetworking { get; set; }

    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--auxiliary-mode")]
    public string? AuxiliaryMode { get; set; }

    [CommandSwitch("--auxiliary-sku")]
    public string? AuxiliarySku { get; set; }

    [CommandSwitch("--dns-servers")]
    public string? DnsServers { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--internal-dns-name")]
    public string? InternalDnsName { get; set; }

    [CommandSwitch("--ip-configurations")]
    public string? IpConfigurations { get; set; }

    [BooleanCommandSwitch("--ip-forwarding")]
    public bool? IpForwarding { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--network-security-group")]
    public string? NetworkSecurityGroup { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

