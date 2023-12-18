using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "midb", "ltr-backup", "restore")]
public record AzSqlMidbLtrBackupRestoreOptions(
[property: CommandSwitch("--backup-id")] string BackupId,
[property: CommandSwitch("--dest-database")] string DestDatabase,
[property: CommandSwitch("--dest-mi")] string DestMi,
[property: CommandSwitch("--dest-resource-group")] string DestResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}