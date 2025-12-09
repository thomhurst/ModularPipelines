using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dla", "catalog", "table", "list")]
public record AzDlaCatalogTableListOptions(
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
}