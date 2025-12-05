using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "tag-templates", "fields", "delete")]
public record GcloudDataCatalogTagTemplatesFieldsDeleteOptions(
[property: CliArgument] string Field,
[property: CliArgument] string Location,
[property: CliArgument] string TagTemplate
) : GcloudOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }
}