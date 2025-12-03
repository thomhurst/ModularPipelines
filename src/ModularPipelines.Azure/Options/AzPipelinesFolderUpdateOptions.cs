using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pipelines", "folder", "update")]
public record AzPipelinesFolderUpdateOptions(
[property: CliOption("--path")] string Path
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--new-description")]
    public string? NewDescription { get; set; }

    [CliOption("--new-path")]
    public string? NewPath { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }
}