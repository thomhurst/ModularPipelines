using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dataprotection", "backup-vault", "update")]
public record AzDataprotectionBackupVaultUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--azure-monitor-alerts-for-job-failures")]
    public string? AzureMonitorAlertsForJobFailures { get; set; }

    [CliOption("--cross-subscription-restore-state")]
    public string? CrossSubscriptionRestoreState { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--immutability-state")]
    public string? ImmutabilityState { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--retention-duration-in-days")]
    public string? RetentionDurationInDays { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--soft-delete-state")]
    public string? SoftDeleteState { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }
}