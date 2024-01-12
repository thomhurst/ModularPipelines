using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("run", "jobs", "executions", "tasks", "describe")]
public record GcloudRunJobsExecutionsTasksDescribeOptions(
[property: PositionalArgument] string Task
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}