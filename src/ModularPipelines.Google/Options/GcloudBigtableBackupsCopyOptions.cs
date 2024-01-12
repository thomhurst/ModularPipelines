using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "backups", "copy")]
public record GcloudBigtableBackupsCopyOptions(
[property: CommandSwitch("--destination-backup")] string DestinationBackup,
[property: CommandSwitch("--destination-cluster")] string DestinationCluster,
[property: CommandSwitch("--destination-instance")] string DestinationInstance,
[property: CommandSwitch("--destination-project")] string DestinationProject,
[property: CommandSwitch("--expiration-date")] string ExpirationDate,
[property: CommandSwitch("--retention-period")] string RetentionPeriod,
[property: CommandSwitch("--source-backup")] string SourceBackup,
[property: CommandSwitch("--source-cluster")] string SourceCluster,
[property: CommandSwitch("--source-instance")] string SourceInstance,
[property: CommandSwitch("--source-project")] string SourceProject
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}