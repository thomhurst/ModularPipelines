using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tasks", "queues", "purge")]
public record GcloudTasksQueuesPurgeOptions(
[property: CliArgument] string Queue
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}