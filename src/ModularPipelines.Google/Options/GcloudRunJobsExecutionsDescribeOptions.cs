using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("run", "jobs", "executions", "describe")]
public record GcloudRunJobsExecutionsDescribeOptions(
[property: PositionalArgument] string Execution
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}