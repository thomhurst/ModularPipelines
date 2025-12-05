using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tasks", "queues", "describe")]
public record GcloudTasksQueuesDescribeOptions(
[property: CliArgument] string Queue
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}