using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("filestore", "instances", "restore")]
public record GcloudFilestoreInstancesRestoreOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Zone,
[property: CommandSwitch("--file-share")] string FileShare,
[property: CommandSwitch("--source-backup")] string SourceBackup,
[property: CommandSwitch("--source-backup-region")] string SourceBackupRegion
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}