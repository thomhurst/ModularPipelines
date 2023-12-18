using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-instance", "restore", "initialize-for-data-recovery-as-files")]
public record AzDataprotectionBackupInstanceRestoreInitializeForDataRecoveryAsFilesOptions(
[property: CommandSwitch("--datasource-type")] string DatasourceType,
[property: CommandSwitch("--restore-location")] string RestoreLocation,
[property: CommandSwitch("--source-datastore")] string SourceDatastore,
[property: CommandSwitch("--target-blob-container-url")] string TargetBlobContainerUrl,
[property: CommandSwitch("--target-file-name")] string TargetFileName
) : AzOptions
{
    [CommandSwitch("--point-in-time")]
    public string? PointInTime { get; set; }

    [CommandSwitch("--recovery-point-id")]
    public string? RecoveryPointId { get; set; }

    [CommandSwitch("--rehydration-duration")]
    public string? RehydrationDuration { get; set; }

    [CommandSwitch("--rehydration-priority")]
    public string? RehydrationPriority { get; set; }

    [CommandSwitch("--target-resource-id")]
    public string? TargetResourceId { get; set; }
}