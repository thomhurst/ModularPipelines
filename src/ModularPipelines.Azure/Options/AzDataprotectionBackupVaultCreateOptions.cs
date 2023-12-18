using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-vault", "create")]
public record AzDataprotectionBackupVaultCreateOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--storage-setting")] string StorageSetting,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--azure-monitor-alerts-for-job-failures")]
    public string? AzureMonitorAlertsForJobFailures { get; set; }

    [CommandSwitch("--cross-subscription-restore-state")]
    public string? CrossSubscriptionRestoreState { get; set; }

    [CommandSwitch("--e-tag")]
    public string? ETag { get; set; }

    [CommandSwitch("--immutability-state")]
    public string? ImmutabilityState { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--retention-duration-in-days")]
    public string? RetentionDurationInDays { get; set; }

    [CommandSwitch("--soft-delete-state")]
    public string? SoftDeleteState { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}

