using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "taxonomies", "policy-tags", "get-iam-policy")]
public record GcloudDataCatalogTaxonomiesPolicyTagsGetIamPolicyOptions(
[property: PositionalArgument] string PolicyTag,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Taxonomy
) : GcloudOptions;