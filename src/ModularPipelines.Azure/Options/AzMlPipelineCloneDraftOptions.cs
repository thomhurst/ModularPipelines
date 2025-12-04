using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "pipeline", "clone-draft")]
public record AzMlPipelineCloneDraftOptions : AzOptions
{
    [CliOption("--experiment-name")]
    public string? ExperimentName { get; set; }

    [CliOption("--pipeline-draft-id")]
    public string? PipelineDraftId { get; set; }

    [CliOption("--pipeline-id")]
    public string? PipelineId { get; set; }

    [CliOption("--pipeline-run-id")]
    public string? PipelineRunId { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}