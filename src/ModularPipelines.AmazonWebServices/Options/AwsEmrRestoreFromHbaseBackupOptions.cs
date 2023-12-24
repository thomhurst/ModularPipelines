using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "restore-from-hbase-backup")]
public record AwsEmrRestoreFromHbaseBackupOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId,
[property: CommandSwitch("--dir")] string Dir
) : AwsOptions
{
    [CommandSwitch("--backup-version")]
    public string? BackupVersion { get; set; }
}