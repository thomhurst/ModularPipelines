using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("run", "jobs", "describe")]
public record GcloudRunJobsDescribeOptions(
[property: PositionalArgument] string Job
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}