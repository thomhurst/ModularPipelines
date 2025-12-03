using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataprotection", "backup-instance", "restore", "initialize-for-item-recovery")]
public record AzDataprotectionBackupInstanceRestoreInitializeForItemRecoveryOptions(
[property: CliOption("--datasource-type")] string DatasourceType,
[property: CliOption("--restore-location")] string RestoreLocation,
[property: CliOption("--source-datastore")] string SourceDatastore
) : AzOptions
{
    [CliOption("--backup-instance-id")]
    public string? BackupInstanceId { get; set; }

    [CliOption("--container-list")]
    public string? ContainerList { get; set; }

    [CliOption("--from-prefix-pattern")]
    public string? FromPrefixPattern { get; set; }

    [CliOption("--point-in-time")]
    public string? PointInTime { get; set; }

    [CliOption("--recovery-point-id")]
    public string? RecoveryPointId { get; set; }

    [CliOption("--restore-configuration")]
    public string? RestoreConfiguration { get; set; }

    [CliOption("--target-resource-id")]
    public string? TargetResourceId { get; set; }

    [CliOption("--to-prefix-pattern")]
    public string? ToPrefixPattern { get; set; }
}