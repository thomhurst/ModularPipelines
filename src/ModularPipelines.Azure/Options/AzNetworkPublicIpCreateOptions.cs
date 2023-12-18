using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "public-ip", "create")]
public record AzNetworkPublicIpCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--allocation-method")]
    public string? AllocationMethod { get; set; }

    [CommandSwitch("--ddos-protection-mode")]
    public string? DdosProtectionMode { get; set; }

    [CommandSwitch("--ddos-protection-plan")]
    public string? DdosProtectionPlan { get; set; }

    [CommandSwitch("--dns-name")]
    public string? DnsName { get; set; }

    [CommandSwitch("--dns-name-scope")]
    public string? DnsNameScope { get; set; }

    [CommandSwitch("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CommandSwitch("--idle-timeout")]
    public string? IdleTimeout { get; set; }

    [CommandSwitch("--ip-address")]
    public string? IpAddress { get; set; }

    [CommandSwitch("--ip-tags")]
    public string? IpTags { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--public-ip-prefix")]
    public string? PublicIpPrefix { get; set; }

    [CommandSwitch("--reverse-fqdn")]
    public string? ReverseFqdn { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}