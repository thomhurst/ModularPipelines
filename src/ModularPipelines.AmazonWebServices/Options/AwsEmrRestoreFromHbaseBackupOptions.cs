using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "restore-from-hbase-backup")]
public record AwsEmrRestoreFromHbaseBackupOptions(
[property: CliOption("--cluster-id")] string ClusterId,
[property: CliOption("--dir")] string Dir
) : AwsOptions
{
    [CliOption("--backup-version")]
    public string? BackupVersion { get; set; }
}