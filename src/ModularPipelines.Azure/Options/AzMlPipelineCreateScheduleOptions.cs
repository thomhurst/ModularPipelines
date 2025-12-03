using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "pipeline", "create-schedule")]
public record AzMlPipelineCreateScheduleOptions(
[property: CliOption("--experiment-name")] string ExperimentName,
[property: CliOption("--name")] string Name,
[property: CliOption("--pipeline-id")] string PipelineId
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--schedule-yaml")]
    public string? ScheduleYaml { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}