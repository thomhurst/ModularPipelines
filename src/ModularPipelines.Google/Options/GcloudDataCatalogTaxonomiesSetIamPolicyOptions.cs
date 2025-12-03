using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "taxonomies", "set-iam-policy")]
public record GcloudDataCatalogTaxonomiesSetIamPolicyOptions(
[property: CliArgument] string Taxonomy,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;