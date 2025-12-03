using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "pipeline", "create")]
public record AzMlPipelineCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--pipeline-yaml")] string PipelineYaml
) : AzOptions
{
    [CliOption("--continue")]
    public string? Continue { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--output-file")]
    public string? OutputFile { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}