using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "tags", "update")]
public record GcloudDataCatalogTagsUpdateOptions(
[property: PositionalArgument] string Tag,
[property: PositionalArgument] string Entry,
[property: PositionalArgument] string EntryGroup,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--tag-file")] string TagFile,
[property: CommandSwitch("--tag-template")] string TagTemplate,
[property: CommandSwitch("--tag-template-location")] string TagTemplateLocation,
[property: CommandSwitch("--tag-template-project")] string TagTemplateProject
) : GcloudOptions;