using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "taxonomies", "export")]
public record GcloudDataCatalogTaxonomiesExportOptions(
[property: CliArgument] string Taxonomies,
[property: CliOption("--location")] string Location
) : GcloudOptions;