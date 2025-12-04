using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dla", "catalog", "procedure", "show")]
public record AzDlaCatalogProcedureShowOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--procedure-name")] string ProcedureName,
[property: CliOption("--schema-name")] string SchemaName
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}