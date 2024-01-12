using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("run", "jobs", "delete")]
public record GcloudRunJobsDeleteOptions(
[property: PositionalArgument] string Job
) : GcloudOptions
{
    [CommandSwitch("--[no-]async")]
    public string[]? NoAsync { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}