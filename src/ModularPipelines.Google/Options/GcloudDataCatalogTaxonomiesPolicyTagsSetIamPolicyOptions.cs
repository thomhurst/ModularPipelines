using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "taxonomies", "policy-tags", "set-iam-policy")]
public record GcloudDataCatalogTaxonomiesPolicyTagsSetIamPolicyOptions(
[property: CliArgument] string PolicyTag,
[property: CliArgument] string Location,
[property: CliArgument] string Taxonomy,
[property: CliArgument] string PolicyFile
) : GcloudOptions;