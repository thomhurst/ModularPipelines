using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("run", "jobs", "executions", "describe")]
public record GcloudRunJobsExecutionsDescribeOptions(
[property: CliArgument] string Execution
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}