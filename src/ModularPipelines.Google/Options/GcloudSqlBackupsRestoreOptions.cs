using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "backups", "restore")]
public record GcloudSqlBackupsRestoreOptions(
[property: PositionalArgument] string Id,
[property: CommandSwitch("--restore-instance")] string RestoreInstance
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--backup-instance")]
    public string? BackupInstance { get; set; }
}