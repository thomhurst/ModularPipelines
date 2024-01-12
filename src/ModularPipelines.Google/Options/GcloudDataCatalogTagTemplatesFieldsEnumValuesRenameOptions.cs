using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("data-catalog", "tag-templates", "fields", "enum-values", "rename")]
public record GcloudDataCatalogTagTemplatesFieldsEnumValuesRenameOptions(
[property: PositionalArgument] string EnumValue,
[property: PositionalArgument] string Field,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string TagTemplate,
[property: CommandSwitch("--new-id")] string NewId
) : GcloudOptions;