using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "taxonomies", "policy-tags", "set-iam-policy")]
public record GcloudDataCatalogTaxonomiesPolicyTagsSetIamPolicyOptions(
[property: PositionalArgument] string PolicyTag,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Taxonomy,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;