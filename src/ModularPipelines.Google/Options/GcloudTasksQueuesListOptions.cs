using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tasks", "queues", "list")]
public record GcloudTasksQueuesListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}