using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dla", "catalog", "table-type", "show")]
public record AzDlaCatalogTableTypeShowOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--schema-name")] string SchemaName,
[property: CliOption("--table-type-name")] string TableTypeName
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}