using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("run", "jobs", "deploy")]
public record GcloudRunJobsDeployOptions(
[property: PositionalArgument] string Job
) : GcloudOptions
{
    [CommandSwitch("--args")]
    public string[]? Args { get; set; }

    [CommandSwitch("--binary-authorization")]
    public string? BinaryAuthorization { get; set; }

    [CommandSwitch("--breakglass")]
    public string? Breakglass { get; set; }

    [CommandSwitch("--command")]
    public string[]? Command { get; set; }

    [CommandSwitch("--cpu")]
    public string? Cpu { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--max-retries")]
    public string? MaxRetries { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [CommandSwitch("--parallelism")]
    public string? Parallelism { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [CommandSwitch("--set-cloudsql-instances")]
    public string[]? SetCloudsqlInstances { get; set; }

    [CommandSwitch("--set-secrets")]
    public string[]? SetSecrets { get; set; }

    [CommandSwitch("--task-timeout")]
    public string? TaskTimeout { get; set; }

    [CommandSwitch("--tasks")]
    public string? Tasks { get; set; }

    [CommandSwitch("--vpc-connector")]
    public string? VpcConnector { get; set; }

    [CommandSwitch("--vpc-egress")]
    public string? VpcEgress { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--execute-now")]
    public bool? ExecuteNow { get; set; }

    [BooleanCommandSwitch("--wait")]
    public bool? Wait { get; set; }

    [BooleanCommandSwitch("--clear-env-vars")]
    public bool? ClearEnvVars { get; set; }

    [CommandSwitch("--env-vars-file")]
    public string? EnvVarsFile { get; set; }

    [CommandSwitch("--set-env-vars")]
    public IEnumerable<KeyValue>? SetEnvVars { get; set; }

    [CommandSwitch("--remove-env-vars")]
    public string[]? RemoveEnvVars { get; set; }

    [CommandSwitch("--update-env-vars")]
    public IEnumerable<KeyValue>? UpdateEnvVars { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }
}