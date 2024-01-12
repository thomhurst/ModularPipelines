using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tasks", "queues", "pause")]
public record GcloudTasksQueuesPauseOptions(
[property: PositionalArgument] string Queue
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}