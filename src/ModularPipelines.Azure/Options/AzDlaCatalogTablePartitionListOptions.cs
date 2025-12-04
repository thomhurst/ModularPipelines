using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dla", "catalog", "table-partition", "list")]
public record AzDlaCatalogTablePartitionListOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--schema-name")] string SchemaName,
[property: CliOption("--table-name")] string TableName
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--count")]
    public int? Count { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--orderby")]
    public string? Orderby { get; set; }

    [CliOption("--select")]
    public string? Select { get; set; }

    [CliOption("--skip")]
    public string? Skip { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}