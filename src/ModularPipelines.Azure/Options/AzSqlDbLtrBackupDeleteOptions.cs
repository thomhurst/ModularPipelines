using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "db", "ltr-backup", "delete")]
public record AzSqlDbLtrBackupDeleteOptions(
[property: CliOption("--database")] string Database,
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--server")] string Server
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}