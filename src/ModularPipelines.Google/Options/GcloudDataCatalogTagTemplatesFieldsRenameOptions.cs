using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "tag-templates", "fields", "rename")]
public record GcloudDataCatalogTagTemplatesFieldsRenameOptions(
[property: CliArgument] string Field,
[property: CliArgument] string Location,
[property: CliArgument] string TagTemplate,
[property: CliOption("--new-id")] string NewId
) : GcloudOptions;