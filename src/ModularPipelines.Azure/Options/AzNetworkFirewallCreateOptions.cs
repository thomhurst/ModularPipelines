using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "firewall", "create")]
public record AzNetworkFirewallCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--allow-active-ftp")]
    public bool? AllowActiveFtp { get; set; }

    [CliOption("--conf-name")]
    public string? ConfName { get; set; }

    [CliOption("--count")]
    public int? Count { get; set; }

    [CliOption("--dns-servers")]
    public string? DnsServers { get; set; }

    [CliFlag("--enable-dns-proxy")]
    public bool? EnableDnsProxy { get; set; }

    [CliFlag("--enable-fat-flow-logging")]
    public bool? EnableFatFlowLogging { get; set; }

    [CliFlag("--enable-udp-log-optimization")]
    public bool? EnableUdpLogOptimization { get; set; }

    [CliOption("--firewall-policy")]
    public string? FirewallPolicy { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--m-conf-name")]
    public string? MConfName { get; set; }

    [CliOption("--m-public-ip")]
    public string? MPublicIp { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--private-ranges")]
    public string? PrivateRanges { get; set; }

    [CliOption("--public-ip")]
    public string? PublicIp { get; set; }

    [CliOption("--route-server-id")]
    public string? RouteServerId { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--threat-intel-mode")]
    public string? ThreatIntelMode { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--vhub")]
    public string? Vhub { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }

    [CliOption("--zones")]
    public string? Zones { get; set; }
}