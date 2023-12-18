using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "pipeline", "update-schedule")]
public record AzMlPipelineUpdateScheduleOptions(
[property: CommandSwitch("--schedule-id")] string ScheduleId
) : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--schedule-yaml")]
    public string? ScheduleYaml { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}