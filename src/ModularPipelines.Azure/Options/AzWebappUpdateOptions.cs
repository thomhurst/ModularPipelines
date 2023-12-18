using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "update")]
public record AzWebappUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [BooleanCommandSwitch("--client-affinity-enabled")]
    public bool? ClientAffinityEnabled { get; set; }

    [BooleanCommandSwitch("--force-dns-registration")]
    public bool? ForceDnsRegistration { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [BooleanCommandSwitch("--https-only")]
    public bool? HttpsOnly { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--minimum-elastic-instance-count")]
    public int? MinimumElasticInstanceCount { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--prewarmed-instance-count")]
    public int? PrewarmedInstanceCount { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [BooleanCommandSwitch("--skip-custom-domain-verification")]
    public bool? SkipCustomDomainVerification { get; set; }

    [BooleanCommandSwitch("--skip-dns-registration")]
    public bool? SkipDnsRegistration { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [BooleanCommandSwitch("--ttl-in-seconds")]
    public bool? TtlInSeconds { get; set; }
}