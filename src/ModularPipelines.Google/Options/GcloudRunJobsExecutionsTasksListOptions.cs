using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("run", "jobs", "executions", "tasks", "list")]
public record GcloudRunJobsExecutionsTasksListOptions(
[property: CommandSwitch("--execution")] string Execution
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [BooleanCommandSwitch("--succeeded")]
    public bool? Succeeded { get; set; }
}