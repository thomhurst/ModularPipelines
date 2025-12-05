using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("filestore", "instances", "restore")]
public record GcloudFilestoreInstancesRestoreOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Zone,
[property: CliOption("--file-share")] string FileShare,
[property: CliOption("--source-backup")] string SourceBackup,
[property: CliOption("--source-backup-region")] string SourceBackupRegion
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}