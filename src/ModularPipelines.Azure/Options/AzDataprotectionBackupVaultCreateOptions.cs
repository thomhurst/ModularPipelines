using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dataprotection", "backup-vault", "create")]
public record AzDataprotectionBackupVaultCreateOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-setting")] string StorageSetting,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliOption("--azure-monitor-alerts-for-job-failures")]
    public string? AzureMonitorAlertsForJobFailures { get; set; }

    [CliOption("--cross-subscription-restore-state")]
    public string? CrossSubscriptionRestoreState { get; set; }

    [CliOption("--e-tag")]
    public string? ETag { get; set; }

    [CliOption("--immutability-state")]
    public string? ImmutabilityState { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--retention-duration-in-days")]
    public string? RetentionDurationInDays { get; set; }

    [CliOption("--soft-delete-state")]
    public string? SoftDeleteState { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}