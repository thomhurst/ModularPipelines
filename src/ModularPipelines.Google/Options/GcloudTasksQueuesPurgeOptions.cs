using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tasks", "queues", "purge")]
public record GcloudTasksQueuesPurgeOptions(
[property: PositionalArgument] string Queue
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}