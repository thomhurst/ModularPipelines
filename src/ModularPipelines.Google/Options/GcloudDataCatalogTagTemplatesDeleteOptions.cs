using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "tag-templates", "delete")]
public record GcloudDataCatalogTagTemplatesDeleteOptions(
[property: CliArgument] string TagTemplate,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }
}