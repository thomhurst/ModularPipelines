using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("run", "jobs", "executions", "tasks", "list")]
public record GcloudRunJobsExecutionsTasksListOptions(
[property: CliOption("--execution")] string Execution
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }

    [CliFlag("--succeeded")]
    public bool? Succeeded { get; set; }
}