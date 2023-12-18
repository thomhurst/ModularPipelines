using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "vault", "backup-properties", "set")]
public record AzBackupVaultBackupPropertiesSetOptions : AzOptions
{
    [CommandSwitch("--backup-storage-redundancy")]
    public string? BackupStorageRedundancy { get; set; }

    [CommandSwitch("--classic-alerts")]
    public string? ClassicAlerts { get; set; }

    [BooleanCommandSwitch("--cross-region-restore-flag")]
    public bool? CrossRegionRestoreFlag { get; set; }

    [CommandSwitch("--hybrid-backup-security-features")]
    public string? HybridBackupSecurityFeatures { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--job-failure-alerts")]
    public string? JobFailureAlerts { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--soft-delete-duration")]
    public string? SoftDeleteDuration { get; set; }

    [CommandSwitch("--soft-delete-feature-state")]
    public string? SoftDeleteFeatureState { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tenant-id")]
    public string? TenantId { get; set; }
}