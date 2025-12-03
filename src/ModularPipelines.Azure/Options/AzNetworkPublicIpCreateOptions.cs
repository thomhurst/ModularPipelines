using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "public-ip", "create")]
public record AzNetworkPublicIpCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--allocation-method")]
    public string? AllocationMethod { get; set; }

    [CliOption("--ddos-protection-mode")]
    public string? DdosProtectionMode { get; set; }

    [CliOption("--ddos-protection-plan")]
    public string? DdosProtectionPlan { get; set; }

    [CliOption("--dns-name")]
    public string? DnsName { get; set; }

    [CliOption("--dns-name-scope")]
    public string? DnsNameScope { get; set; }

    [CliOption("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CliOption("--idle-timeout")]
    public string? IdleTimeout { get; set; }

    [CliOption("--ip-address")]
    public string? IpAddress { get; set; }

    [CliOption("--ip-tags")]
    public string? IpTags { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--public-ip-prefix")]
    public string? PublicIpPrefix { get; set; }

    [CliOption("--reverse-fqdn")]
    public string? ReverseFqdn { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}