using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "nic", "update")]
public record AzNetworkNicUpdateOptions : AzOptions
{
    [CliFlag("--accelerated-networking")]
    public bool? AcceleratedNetworking { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--auxiliary-mode")]
    public string? AuxiliaryMode { get; set; }

    [CliOption("--auxiliary-sku")]
    public string? AuxiliarySku { get; set; }

    [CliOption("--dns-servers")]
    public string? DnsServers { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--internal-dns-name")]
    public string? InternalDnsName { get; set; }

    [CliOption("--ip-configurations")]
    public string? IpConfigurations { get; set; }

    [CliFlag("--ip-forwarding")]
    public bool? IpForwarding { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--network-security-group")]
    public string? NetworkSecurityGroup { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}