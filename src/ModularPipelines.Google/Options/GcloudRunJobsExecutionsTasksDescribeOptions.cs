using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("run", "jobs", "executions", "tasks", "describe")]
public record GcloudRunJobsExecutionsTasksDescribeOptions(
[property: CliArgument] string Task
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}