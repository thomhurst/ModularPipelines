using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "taxonomies", "set-iam-policy")]
public record GcloudDataCatalogTaxonomiesSetIamPolicyOptions(
[property: PositionalArgument] string Taxonomy,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;