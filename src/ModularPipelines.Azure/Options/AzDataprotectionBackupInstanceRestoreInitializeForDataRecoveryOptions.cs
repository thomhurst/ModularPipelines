using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataprotection", "backup-instance", "restore", "initialize-for-data-recovery")]
public record AzDataprotectionBackupInstanceRestoreInitializeForDataRecoveryOptions(
[property: CliOption("--datasource-type")] string DatasourceType,
[property: CliOption("--restore-location")] string RestoreLocation,
[property: CliOption("--source-datastore")] string SourceDatastore
) : AzOptions
{
    [CliOption("--backup-instance-id")]
    public string? BackupInstanceId { get; set; }

    [CliOption("--point-in-time")]
    public string? PointInTime { get; set; }

    [CliOption("--recovery-point-id")]
    public string? RecoveryPointId { get; set; }

    [CliOption("--rehydration-duration")]
    public string? RehydrationDuration { get; set; }

    [CliOption("--rehydration-priority")]
    public string? RehydrationPriority { get; set; }

    [CliOption("--restore-configuration")]
    public string? RestoreConfiguration { get; set; }

    [CliOption("--secret-store-type")]
    public string? SecretStoreType { get; set; }

    [CliOption("--secret-store-uri")]
    public string? SecretStoreUri { get; set; }

    [CliOption("--target-resource-id")]
    public string? TargetResourceId { get; set; }
}