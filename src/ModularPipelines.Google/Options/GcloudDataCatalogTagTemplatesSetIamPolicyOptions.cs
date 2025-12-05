using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "tag-templates", "set-iam-policy")]
public record GcloudDataCatalogTagTemplatesSetIamPolicyOptions(
[property: CliArgument] string TagTemplate,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;