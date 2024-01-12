using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "tag-templates", "create")]
public record GcloudDataCatalogTagTemplatesCreateOptions(
[property: PositionalArgument] string TagTemplate,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--field")] string[] Field
) : GcloudOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }
}