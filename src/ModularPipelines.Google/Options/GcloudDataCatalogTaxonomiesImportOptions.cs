using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "taxonomies", "import")]
public record GcloudDataCatalogTaxonomiesImportOptions(
[property: CliArgument] string Taxonomies,
[property: CliOption("--location")] string Location
) : GcloudOptions;