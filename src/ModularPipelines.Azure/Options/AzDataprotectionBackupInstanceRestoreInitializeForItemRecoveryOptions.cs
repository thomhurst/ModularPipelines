using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-instance", "restore", "initialize-for-item-recovery")]
public record AzDataprotectionBackupInstanceRestoreInitializeForItemRecoveryOptions(
[property: CommandSwitch("--datasource-type")] string DatasourceType,
[property: CommandSwitch("--restore-location")] string RestoreLocation,
[property: CommandSwitch("--source-datastore")] string SourceDatastore
) : AzOptions
{
    [CommandSwitch("--backup-instance-id")]
    public string? BackupInstanceId { get; set; }

    [CommandSwitch("--container-list")]
    public string? ContainerList { get; set; }

    [CommandSwitch("--from-prefix-pattern")]
    public string? FromPrefixPattern { get; set; }

    [CommandSwitch("--point-in-time")]
    public string? PointInTime { get; set; }

    [CommandSwitch("--recovery-point-id")]
    public string? RecoveryPointId { get; set; }

    [CommandSwitch("--restore-configuration")]
    public string? RestoreConfiguration { get; set; }

    [CommandSwitch("--target-resource-id")]
    public string? TargetResourceId { get; set; }

    [CommandSwitch("--to-prefix-pattern")]
    public string? ToPrefixPattern { get; set; }
}

