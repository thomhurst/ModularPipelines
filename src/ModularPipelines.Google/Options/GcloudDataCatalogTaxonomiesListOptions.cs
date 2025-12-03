using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "taxonomies", "list")]
public record GcloudDataCatalogTaxonomiesListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;