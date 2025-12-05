using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "taxonomies", "policy-tags", "list")]
public record GcloudDataCatalogTaxonomiesPolicyTagsListOptions(
[property: CliOption("--taxonomy")] string Taxonomy,
[property: CliOption("--location")] string Location
) : GcloudOptions;