using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "firewall", "create")]
public record AzNetworkFirewallCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--allow-active-ftp")]
    public bool? AllowActiveFtp { get; set; }

    [CommandSwitch("--conf-name")]
    public string? ConfName { get; set; }

    [CommandSwitch("--count")]
    public int? Count { get; set; }

    [CommandSwitch("--dns-servers")]
    public string? DnsServers { get; set; }

    [BooleanCommandSwitch("--enable-dns-proxy")]
    public bool? EnableDnsProxy { get; set; }

    [BooleanCommandSwitch("--enable-fat-flow-logging")]
    public bool? EnableFatFlowLogging { get; set; }

    [BooleanCommandSwitch("--enable-udp-log-optimization")]
    public bool? EnableUdpLogOptimization { get; set; }

    [CommandSwitch("--firewall-policy")]
    public string? FirewallPolicy { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--m-conf-name")]
    public string? MConfName { get; set; }

    [CommandSwitch("--m-public-ip")]
    public string? MPublicIp { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--private-ranges")]
    public string? PrivateRanges { get; set; }

    [CommandSwitch("--public-ip")]
    public string? PublicIp { get; set; }

    [CommandSwitch("--route-server-id")]
    public string? RouteServerId { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--threat-intel-mode")]
    public string? ThreatIntelMode { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--vhub")]
    public string? Vhub { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }

    [CommandSwitch("--zones")]
    public string? Zones { get; set; }
}