using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai-platform", "jobs", "describe")]
public record GcloudAiPlatformJobsDescribeOptions(
[property: PositionalArgument] string Job
) : GcloudOptions
{
    [BooleanCommandSwitch("--summarize")]
    public bool? Summarize { get; set; }
}