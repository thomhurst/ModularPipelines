using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml-engine", "jobs", "describe")]
public record GcloudMlEngineJobsDescribeOptions(
[property: PositionalArgument] string Job
) : GcloudOptions
{
    [BooleanCommandSwitch("--summarize")]
    public bool? Summarize { get; set; }
}