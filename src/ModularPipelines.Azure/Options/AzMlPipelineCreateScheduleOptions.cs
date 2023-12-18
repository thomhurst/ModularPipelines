using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "pipeline", "create-schedule")]
public record AzMlPipelineCreateScheduleOptions(
[property: CommandSwitch("--experiment-name")] string ExperimentName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--pipeline-id")] string PipelineId
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--schedule-yaml")]
    public string? ScheduleYaml { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}

