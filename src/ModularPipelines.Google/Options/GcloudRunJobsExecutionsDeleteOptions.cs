using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("run", "jobs", "executions", "delete")]
public record GcloudRunJobsExecutionsDeleteOptions(
[property: PositionalArgument] string Execution
) : GcloudOptions
{
    [CommandSwitch("--[no-]async")]
    public string[]? NoAsync { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}