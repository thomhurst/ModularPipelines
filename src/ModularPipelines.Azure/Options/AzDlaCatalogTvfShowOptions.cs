using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dla", "catalog", "tvf", "show")]
public record AzDlaCatalogTvfShowOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--schema-name")] string SchemaName,
[property: CliOption("--table-valued-function-name")] string TableValuedFunctionName
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}