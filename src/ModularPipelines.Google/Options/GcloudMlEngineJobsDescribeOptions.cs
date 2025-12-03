using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml-engine", "jobs", "describe")]
public record GcloudMlEngineJobsDescribeOptions(
[property: CliArgument] string Job
) : GcloudOptions
{
    [CliFlag("--summarize")]
    public bool? Summarize { get; set; }
}