using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("run", "jobs", "executions", "list")]
public record GcloudRunJobsExecutionsListOptions : GcloudOptions
{
    [CommandSwitch("--job")]
    public string? Job { get; set; }

    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}