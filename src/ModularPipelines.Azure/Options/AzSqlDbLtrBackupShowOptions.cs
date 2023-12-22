using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "db", "ltr-backup", "show")]
public record AzSqlDbLtrBackupShowOptions(
[property: CommandSwitch("--database")] string Database,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--server")] string Server
) : AzOptions;