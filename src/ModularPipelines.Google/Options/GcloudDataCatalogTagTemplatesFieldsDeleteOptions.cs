using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "tag-templates", "fields", "delete")]
public record GcloudDataCatalogTagTemplatesFieldsDeleteOptions(
[property: PositionalArgument] string Field,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string TagTemplate
) : GcloudOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}