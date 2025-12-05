using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "taxonomies", "policy-tags", "remove-iam-policy-binding")]
public record GcloudDataCatalogTaxonomiesPolicyTagsRemoveIamPolicyBindingOptions(
[property: CliArgument] string PolicyTag,
[property: CliArgument] string Location,
[property: CliArgument] string Taxonomy,
[property: CliOption("--member")] string Member,
[property: CliOption("--role")] string Role
) : GcloudOptions;