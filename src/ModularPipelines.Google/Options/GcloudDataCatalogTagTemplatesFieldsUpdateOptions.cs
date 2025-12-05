using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("data-catalog", "tag-templates", "fields", "update")]
public record GcloudDataCatalogTagTemplatesFieldsUpdateOptions(
[property: CliArgument] string Field,
[property: CliArgument] string Location,
[property: CliArgument] string TagTemplate
) : GcloudOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--enum-values")]
    public string[]? EnumValues { get; set; }

    [CliFlag("--required")]
    public bool? Required { get; set; }
}