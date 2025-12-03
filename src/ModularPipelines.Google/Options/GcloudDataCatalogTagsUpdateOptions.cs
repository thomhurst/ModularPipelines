using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "tags", "update")]
public record GcloudDataCatalogTagsUpdateOptions(
[property: CliArgument] string Tag,
[property: CliArgument] string Entry,
[property: CliArgument] string EntryGroup,
[property: CliArgument] string Location,
[property: CliOption("--tag-file")] string TagFile,
[property: CliOption("--tag-template")] string TagTemplate,
[property: CliOption("--tag-template-location")] string TagTemplateLocation,
[property: CliOption("--tag-template-project")] string TagTemplateProject
) : GcloudOptions;