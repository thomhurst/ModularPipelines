using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "midb", "ltr-backup", "restore")]
public record AzSqlMidbLtrBackupRestoreOptions(
[property: CliOption("--backup-id")] string BackupId,
[property: CliOption("--dest-database")] string DestDatabase,
[property: CliOption("--dest-mi")] string DestMi,
[property: CliOption("--dest-resource-group")] string DestResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}