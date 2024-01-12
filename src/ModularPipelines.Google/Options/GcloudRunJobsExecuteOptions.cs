using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("run", "jobs", "execute")]
public record GcloudRunJobsExecuteOptions(
[property: PositionalArgument] string Job
) : GcloudOptions
{
    [CommandSwitch("--args")]
    public string[]? Args { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--task-timeout")]
    public string? TaskTimeout { get; set; }

    [CommandSwitch("--tasks")]
    public string? Tasks { get; set; }

    [CommandSwitch("--update-env-vars")]
    public IEnumerable<KeyValue>? UpdateEnvVars { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--wait")]
    public bool? Wait { get; set; }
}