using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "tags", "create")]
public record GcloudDataCatalogTagsCreateOptions(
[property: CliOption("--tag-file")] string TagFile,
[property: CliOption("--entry")] string Entry,
[property: CliOption("--entry-group")] string EntryGroup,
[property: CliOption("--location")] string Location,
[property: CliOption("--tag-template")] string TagTemplate,
[property: CliOption("--tag-template-location")] string TagTemplateLocation,
[property: CliOption("--tag-template-project")] string TagTemplateProject
) : GcloudOptions
{
    [CliOption("--scope")]
    public string? Scope { get; set; }
}