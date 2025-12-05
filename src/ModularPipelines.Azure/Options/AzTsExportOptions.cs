using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ts", "export")]
public record AzTsExportOptions(
[property: CliOption("--output-folder")] string OutputFolder
) : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--template-spec")]
    public string? TemplateSpec { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}