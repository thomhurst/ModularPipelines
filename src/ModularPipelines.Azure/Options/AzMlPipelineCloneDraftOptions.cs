using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "pipeline", "clone-draft")]
public record AzMlPipelineCloneDraftOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--pipeline-yaml")] string PipelineYaml
) : AzOptions
{
    [CommandSwitch("--experiment-name")]
    public string? ExperimentName { get; set; }

    [CommandSwitch("--pipeline-draft-id")]
    public string? PipelineDraftId { get; set; }

    [CommandSwitch("--pipeline-id")]
    public string? PipelineId { get; set; }

    [CommandSwitch("--pipeline-run-id")]
    public string? PipelineRunId { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}