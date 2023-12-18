using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "db", "ltr-backup", "delete")]
public record AzSqlDbLtrBackupDeleteOptions(
[property: CommandSwitch("--database")] string Database,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--server")] string Server
) : AzOptions
{
    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}