using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "taxonomies", "describe")]
public record GcloudDataCatalogTaxonomiesDescribeOptions(
[property: CliArgument] string Taxonomy,
[property: CliArgument] string Location
) : GcloudOptions;