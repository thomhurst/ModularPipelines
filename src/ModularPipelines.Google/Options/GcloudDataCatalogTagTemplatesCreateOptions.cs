using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "tag-templates", "create")]
public record GcloudDataCatalogTagTemplatesCreateOptions(
[property: CliArgument] string TagTemplate,
[property: CliArgument] string Location,
[property: CliOption("--field")] string[] Field
) : GcloudOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }
}