using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "taxonomies", "policy-tags", "add-iam-policy-binding")]
public record GcloudDataCatalogTaxonomiesPolicyTagsAddIamPolicyBindingOptions(
[property: PositionalArgument] string PolicyTag,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Taxonomy,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;