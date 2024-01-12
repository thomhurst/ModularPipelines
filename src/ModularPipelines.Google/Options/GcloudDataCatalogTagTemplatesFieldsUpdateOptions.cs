using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "tag-templates", "fields", "update")]
public record GcloudDataCatalogTagTemplatesFieldsUpdateOptions(
[property: PositionalArgument] string Field,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string TagTemplate
) : GcloudOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--enum-values")]
    public string[]? EnumValues { get; set; }

    [BooleanCommandSwitch("--required")]
    public bool? Required { get; set; }
}