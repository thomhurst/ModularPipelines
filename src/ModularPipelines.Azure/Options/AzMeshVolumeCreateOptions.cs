using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mesh", "volume", "create")]
public record AzMeshVolumeCreateOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--template-file")]
    public string? TemplateFile { get; set; }

    [CliOption("--template-uri")]
    public string? TemplateUri { get; set; }
}