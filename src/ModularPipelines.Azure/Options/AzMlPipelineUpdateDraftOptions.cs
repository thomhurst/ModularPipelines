using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "pipeline", "update-draft")]
public record AzMlPipelineUpdateDraftOptions : AzOptions
{
    [CliOption("--continue")]
    public string? Continue { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--experiment_name")]
    public string? ExperimentName { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--pipeline-draft-id")]
    public string? PipelineDraftId { get; set; }

    [CliOption("--pipeline-yaml")]
    public string? PipelineYaml { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}