using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "tag-templates", "update")]
public record GcloudDataCatalogTagTemplatesUpdateOptions(
[property: CliArgument] string TagTemplate,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }
}