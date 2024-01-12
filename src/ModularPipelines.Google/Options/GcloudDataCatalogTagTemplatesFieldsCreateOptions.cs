using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "tag-templates", "fields", "create")]
public record GcloudDataCatalogTagTemplatesFieldsCreateOptions(
[property: PositionalArgument] string Field,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string TagTemplate,
[property: CommandSwitch("--type")] string Type
) : GcloudOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }
}