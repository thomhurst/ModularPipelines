using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "pipeline", "clone")]
public record AzMlPipelineCloneOptions(
[property: CliOption("--path")] string Path,
[property: CliOption("--pipeline-run-id")] string PipelineRunId
) : AzOptions
{
    [CliOption("--output-file")]
    public string? OutputFile { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}