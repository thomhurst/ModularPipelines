using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "firewall", "update")]
public record AzNetworkFirewallUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--allow-active-ftp")]
    public bool? AllowActiveFtp { get; set; }

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

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--private-ranges")]
    public string? PrivateRanges { get; set; }

    [CliOption("--public-ips")]
    public string? PublicIps { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--route-server-id")]
    public string? RouteServerId { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--threat-intel-mode")]
    public string? ThreatIntelMode { get; set; }

    [CliOption("--vhub")]
    public string? Vhub { get; set; }

    [CliOption("--zones")]
    public string? Zones { get; set; }
}