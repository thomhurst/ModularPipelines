using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "taxonomies", "get-iam-policy")]
public record GcloudDataCatalogTaxonomiesGetIamPolicyOptions(
[property: CliArgument] string Taxonomy,
[property: CliArgument] string Location
) : GcloudOptions;