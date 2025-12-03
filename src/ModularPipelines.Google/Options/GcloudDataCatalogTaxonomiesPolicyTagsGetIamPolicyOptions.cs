using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "taxonomies", "policy-tags", "get-iam-policy")]
public record GcloudDataCatalogTaxonomiesPolicyTagsGetIamPolicyOptions(
[property: CliArgument] string PolicyTag,
[property: CliArgument] string Location,
[property: CliArgument] string Taxonomy
) : GcloudOptions;