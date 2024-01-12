using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "tag-templates", "describe")]
public record GcloudDataCatalogTagTemplatesDescribeOptions(
[property: PositionalArgument] string TagTemplate,
[property: PositionalArgument] string Location
) : GcloudOptions;