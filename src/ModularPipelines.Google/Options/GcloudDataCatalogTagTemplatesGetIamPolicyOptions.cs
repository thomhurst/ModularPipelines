using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "tag-templates", "get-iam-policy")]
public record GcloudDataCatalogTagTemplatesGetIamPolicyOptions(
[property: CliArgument] string TagTemplate,
[property: CliArgument] string Location
) : GcloudOptions;