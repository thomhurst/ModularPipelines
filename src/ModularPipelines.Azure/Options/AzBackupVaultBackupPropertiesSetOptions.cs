using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "vault", "backup-properties", "set")]
public record AzBackupVaultBackupPropertiesSetOptions : AzOptions
{
    [CliOption("--backup-storage-redundancy")]
    public string? BackupStorageRedundancy { get; set; }

    [CliOption("--classic-alerts")]
    public string? ClassicAlerts { get; set; }

    [CliFlag("--cross-region-restore-flag")]
    public bool? CrossRegionRestoreFlag { get; set; }

    [CliOption("--hybrid-backup-security-features")]
    public string? HybridBackupSecurityFeatures { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--job-failure-alerts")]
    public string? JobFailureAlerts { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--soft-delete-duration")]
    public string? SoftDeleteDuration { get; set; }

    [CliOption("--soft-delete-feature-state")]
    public string? SoftDeleteFeatureState { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tenant-id")]
    public string? TenantId { get; set; }
}