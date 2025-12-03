using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "tag-templates", "fields", "create")]
public record GcloudDataCatalogTagTemplatesFieldsCreateOptions(
[property: CliArgument] string Field,
[property: CliArgument] string Location,
[property: CliArgument] string TagTemplate,
[property: CliOption("--type")] string Type
) : GcloudOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }
}