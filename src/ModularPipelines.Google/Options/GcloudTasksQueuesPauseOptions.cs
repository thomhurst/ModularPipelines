using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tasks", "queues", "pause")]
public record GcloudTasksQueuesPauseOptions(
[property: CliArgument] string Queue
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}