using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tasks", "queues", "resume")]
public record GcloudTasksQueuesResumeOptions(
[property: PositionalArgument] string Queue
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}