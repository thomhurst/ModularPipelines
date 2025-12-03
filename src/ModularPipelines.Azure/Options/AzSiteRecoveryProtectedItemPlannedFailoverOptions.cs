using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("site-recovery", "protected-item", "planned-failover")]
public record AzSiteRecoveryProtectedItemPlannedFailoverOptions : AzOptions
{
    [CliOption("--fabric-name")]
    public string? FabricName { get; set; }

    [CliOption("--failover-direction")]
    public string? FailoverDirection { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--protection-container")]
    public string? ProtectionContainer { get; set; }

    [CliOption("--provider-details")]
    public string? ProviderDetails { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }
}