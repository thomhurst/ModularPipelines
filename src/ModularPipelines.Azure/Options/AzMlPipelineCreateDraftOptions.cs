using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "pipeline", "create-draft")]
public record AzMlPipelineCreateDraftOptions(
[property: CliOption("--experiment_name")] string ExperimentName,
[property: CliOption("--name")] string Name,
[property: CliOption("--pipeline-yaml")] string PipelineYaml
) : AzOptions
{
    [CliOption("--continue")]
    public string? Continue { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--properties")]
    public string? Properties { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}