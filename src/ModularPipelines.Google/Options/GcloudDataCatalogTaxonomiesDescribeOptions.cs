using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "taxonomies", "describe")]
public record GcloudDataCatalogTaxonomiesDescribeOptions(
[property: PositionalArgument] string Taxonomy,
[property: PositionalArgument] string Location
) : GcloudOptions;