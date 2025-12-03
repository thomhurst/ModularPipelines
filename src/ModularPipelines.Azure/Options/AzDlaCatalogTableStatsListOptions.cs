using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dla", "catalog", "table-stats", "list")]
public record AzDlaCatalogTableStatsListOptions(
[property: CliOption("--database-name")] string DatabaseName
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--schema-name")]
    public string? SchemaName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--table-name")]
    public string? TableName { get; set; }
}