using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "tag-templates", "fields", "rename")]
public record GcloudDataCatalogTagTemplatesFieldsRenameOptions(
[property: PositionalArgument] string Field,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string TagTemplate,
[property: CommandSwitch("--new-id")] string NewId
) : GcloudOptions;