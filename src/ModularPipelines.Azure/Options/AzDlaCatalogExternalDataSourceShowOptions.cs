using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dla", "catalog", "external-data-source", "show")]
public record AzDlaCatalogExternalDataSourceShowOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--external-data-source-name")] string ExternalDataSourceName
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}