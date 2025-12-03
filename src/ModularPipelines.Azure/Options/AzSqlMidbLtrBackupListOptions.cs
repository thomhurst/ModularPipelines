using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "midb", "ltr-backup", "list")]
public record AzSqlMidbLtrBackupListOptions(
[property: CliOption("--location")] string Location
) : AzOptions
{
    [CliOption("--database")]
    public string? Database { get; set; }

    [CliOption("--database-state")]
    public string? DatabaseState { get; set; }

    [CliOption("--latest")]
    public string? Latest { get; set; }

    [CliOption("--managed-instance")]
    public string? ManagedInstance { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}