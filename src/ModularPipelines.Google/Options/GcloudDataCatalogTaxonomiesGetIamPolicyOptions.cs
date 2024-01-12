using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "taxonomies", "get-iam-policy")]
public record GcloudDataCatalogTaxonomiesGetIamPolicyOptions(
[property: PositionalArgument] string Taxonomy,
[property: PositionalArgument] string Location
) : GcloudOptions;