using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "tag-templates", "delete")]
public record GcloudDataCatalogTagTemplatesDeleteOptions(
[property: PositionalArgument] string TagTemplate,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}