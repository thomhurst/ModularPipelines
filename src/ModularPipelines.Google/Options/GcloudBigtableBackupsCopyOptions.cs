using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "backups", "copy")]
public record GcloudBigtableBackupsCopyOptions(
[property: CliOption("--destination-backup")] string DestinationBackup,
[property: CliOption("--destination-cluster")] string DestinationCluster,
[property: CliOption("--destination-instance")] string DestinationInstance,
[property: CliOption("--destination-project")] string DestinationProject,
[property: CliOption("--expiration-date")] string ExpirationDate,
[property: CliOption("--retention-period")] string RetentionPeriod,
[property: CliOption("--source-backup")] string SourceBackup,
[property: CliOption("--source-cluster")] string SourceCluster,
[property: CliOption("--source-instance")] string SourceInstance,
[property: CliOption("--source-project")] string SourceProject
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}