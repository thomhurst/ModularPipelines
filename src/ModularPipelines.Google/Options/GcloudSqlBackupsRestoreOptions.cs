using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "backups", "restore")]
public record GcloudSqlBackupsRestoreOptions(
[property: CliArgument] string Id,
[property: CliOption("--restore-instance")] string RestoreInstance
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--backup-instance")]
    public string? BackupInstance { get; set; }
}