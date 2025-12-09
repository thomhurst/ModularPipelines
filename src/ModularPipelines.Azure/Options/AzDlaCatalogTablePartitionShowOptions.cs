using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dla", "catalog", "table-partition", "show")]
public record AzDlaCatalogTablePartitionShowOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--partition-name")] string PartitionName,
[property: CliOption("--schema-name")] string SchemaName,
[property: CliOption("--table-name")] string TableName
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}