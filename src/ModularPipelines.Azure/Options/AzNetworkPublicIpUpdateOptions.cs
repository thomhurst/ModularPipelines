using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "public-ip", "update")]
public record AzNetworkPublicIpUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

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

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--idle-timeout")]
    public string? IdleTimeout { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--ip-tags")]
    public string? IpTags { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--public-ip-prefix")]
    public string? PublicIpPrefix { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--reverse-fqdn")]
    public string? ReverseFqdn { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}