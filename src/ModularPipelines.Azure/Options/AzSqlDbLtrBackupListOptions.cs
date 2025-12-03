using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "db", "ltr-backup", "list")]
public record AzSqlDbLtrBackupListOptions(
[property: CliOption("--location")] string Location
) : AzOptions
{
    [CliOption("--database")]
    public string? Database { get; set; }

    [CliOption("--database-state")]
    public string? DatabaseState { get; set; }

    [CliOption("--latest")]
    public string? Latest { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--server")]
    public string? Server { get; set; }
}