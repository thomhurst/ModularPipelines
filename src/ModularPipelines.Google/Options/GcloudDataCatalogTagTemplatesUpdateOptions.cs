using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "tag-templates", "update")]
public record GcloudDataCatalogTagTemplatesUpdateOptions(
[property: PositionalArgument] string TagTemplate,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }
}