using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "vault", "update")]
public record AzBackupVaultUpdateOptions : AzOptions
{
    [CliOption("--classic-alerts")]
    public string? ClassicAlerts { get; set; }

    [CliOption("--cross-subscription-restore-state")]
    public string? CrossSubscriptionRestoreState { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--immutability-state")]
    public string? ImmutabilityState { get; set; }

    [CliOption("--job-failure-alerts")]
    public string? JobFailureAlerts { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}