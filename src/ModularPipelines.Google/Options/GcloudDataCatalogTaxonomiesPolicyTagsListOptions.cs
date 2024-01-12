using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "taxonomies", "policy-tags", "list")]
public record GcloudDataCatalogTaxonomiesPolicyTagsListOptions(
[property: CommandSwitch("--taxonomy")] string Taxonomy,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;