using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "db", "show-deleted")]
public record AzSqlDbShowDeletedOptions(
[property: CliOption("--restorable-dropped-database-id")] string RestorableDroppedDatabaseId
) : AzOptions
{
    [CliFlag("--expand-keys")]
    public bool? ExpandKeys { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--keys-filter")]
    public string? KeysFilter { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--server")]
    public string? Server { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}