using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "taxonomies", "policy-tags", "describe")]
public record GcloudDataCatalogTaxonomiesPolicyTagsDescribeOptions(
[property: CliArgument] string PolicyTag,
[property: CliArgument] string Location,
[property: CliArgument] string Taxonomy
) : GcloudOptions;