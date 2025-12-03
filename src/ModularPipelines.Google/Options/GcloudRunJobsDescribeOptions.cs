using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("run", "jobs", "describe")]
public record GcloudRunJobsDescribeOptions(
[property: CliArgument] string Job
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}