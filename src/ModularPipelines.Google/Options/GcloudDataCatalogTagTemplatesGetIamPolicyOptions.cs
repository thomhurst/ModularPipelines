using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "tag-templates", "get-iam-policy")]
public record GcloudDataCatalogTagTemplatesGetIamPolicyOptions(
[property: PositionalArgument] string TagTemplate,
[property: PositionalArgument] string Location
) : GcloudOptions;