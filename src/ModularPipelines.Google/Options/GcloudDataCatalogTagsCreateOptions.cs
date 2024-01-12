using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "tags", "create")]
public record GcloudDataCatalogTagsCreateOptions(
[property: CommandSwitch("--tag-file")] string TagFile,
[property: CommandSwitch("--entry")] string Entry,
[property: CommandSwitch("--entry-group")] string EntryGroup,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--tag-template")] string TagTemplate,
[property: CommandSwitch("--tag-template-location")] string TagTemplateLocation,
[property: CommandSwitch("--tag-template-project")] string TagTemplateProject
) : GcloudOptions
{
    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}