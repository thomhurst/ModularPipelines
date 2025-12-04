using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dataprotection", "backup-instance", "restore", "initialize-for-data-recovery-as-files")]
public record AzDataprotectionBackupInstanceRestoreInitializeForDataRecoveryAsFilesOptions(
[property: CliOption("--datasource-type")] string DatasourceType,
[property: CliOption("--restore-location")] string RestoreLocation,
[property: CliOption("--source-datastore")] string SourceDatastore,
[property: CliOption("--target-blob-container-url")] string TargetBlobContainerUrl,
[property: CliOption("--target-file-name")] string TargetFileName
) : AzOptions
{
    [CliOption("--point-in-time")]
    public string? PointInTime { get; set; }

    [CliOption("--recovery-point-id")]
    public string? RecoveryPointId { get; set; }

    [CliOption("--rehydration-duration")]
    public string? RehydrationDuration { get; set; }

    [CliOption("--rehydration-priority")]
    public string? RehydrationPriority { get; set; }

    [CliOption("--target-resource-id")]
    public string? TargetResourceId { get; set; }
}