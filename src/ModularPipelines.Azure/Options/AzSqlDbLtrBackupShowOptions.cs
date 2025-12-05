using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "db", "ltr-backup", "show")]
public record AzSqlDbLtrBackupShowOptions(
[property: CliOption("--database")] string Database,
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--server")] string Server
) : AzOptions;