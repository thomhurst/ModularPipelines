using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-instance", "restore", "initialize-for-data-recovery")]
public record AzDataprotectionBackupInstanceRestoreInitializeForDataRecoveryOptions(
[property: CommandSwitch("--datasource-type")] string DatasourceType,
[property: CommandSwitch("--restore-location")] string RestoreLocation,
[property: CommandSwitch("--source-datastore")] string SourceDatastore
) : AzOptions
{
    [CommandSwitch("--backup-instance-id")]
    public string? BackupInstanceId { get; set; }

    [CommandSwitch("--point-in-time")]
    public string? PointInTime { get; set; }

    [CommandSwitch("--recovery-point-id")]
    public string? RecoveryPointId { get; set; }

    [CommandSwitch("--rehydration-duration")]
    public string? RehydrationDuration { get; set; }

    [CommandSwitch("--rehydration-priority")]
    public string? RehydrationPriority { get; set; }

    [CommandSwitch("--restore-configuration")]
    public string? RestoreConfiguration { get; set; }

    [CommandSwitch("--secret-store-type")]
    public string? SecretStoreType { get; set; }

    [CommandSwitch("--secret-store-uri")]
    public string? SecretStoreUri { get; set; }

    [CommandSwitch("--target-resource-id")]
    public string? TargetResourceId { get; set; }
}

