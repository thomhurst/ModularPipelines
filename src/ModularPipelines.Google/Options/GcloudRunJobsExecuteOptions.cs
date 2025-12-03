using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("run", "jobs", "execute")]
public record GcloudRunJobsExecuteOptions(
[property: CliArgument] string Job
) : GcloudOptions
{
    [CliOption("--args")]
    public string[]? Args { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--task-timeout")]
    public string? TaskTimeout { get; set; }

    [CliOption("--tasks")]
    public string? Tasks { get; set; }

    [CliOption("--update-env-vars")]
    public IEnumerable<KeyValue>? UpdateEnvVars { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--wait")]
    public bool? Wait { get; set; }
}