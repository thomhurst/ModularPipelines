using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai-platform", "jobs", "describe")]
public record GcloudAiPlatformJobsDescribeOptions(
[property: CliArgument] string Job
) : GcloudOptions
{
    [CliFlag("--summarize")]
    public bool? Summarize { get; set; }
}