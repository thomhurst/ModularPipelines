using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("run", "jobs", "deploy")]
public record GcloudRunJobsDeployOptions(
[property: CliArgument] string Job
) : GcloudOptions
{
    [CliOption("--args")]
    public string[]? Args { get; set; }

    [CliOption("--binary-authorization")]
    public string? BinaryAuthorization { get; set; }

    [CliOption("--breakglass")]
    public string? Breakglass { get; set; }

    [CliOption("--command")]
    public string[]? Command { get; set; }

    [CliOption("--cpu")]
    public string? Cpu { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--max-retries")]
    public string? MaxRetries { get; set; }

    [CliOption("--memory")]
    public string? Memory { get; set; }

    [CliOption("--parallelism")]
    public string? Parallelism { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliOption("--set-cloudsql-instances")]
    public string[]? SetCloudsqlInstances { get; set; }

    [CliOption("--set-secrets")]
    public string[]? SetSecrets { get; set; }

    [CliOption("--task-timeout")]
    public string? TaskTimeout { get; set; }

    [CliOption("--tasks")]
    public string? Tasks { get; set; }

    [CliOption("--vpc-connector")]
    public string? VpcConnector { get; set; }

    [CliOption("--vpc-egress")]
    public string? VpcEgress { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--execute-now")]
    public bool? ExecuteNow { get; set; }

    [CliFlag("--wait")]
    public bool? Wait { get; set; }

    [CliFlag("--clear-env-vars")]
    public bool? ClearEnvVars { get; set; }

    [CliOption("--env-vars-file")]
    public string? EnvVarsFile { get; set; }

    [CliOption("--set-env-vars")]
    public IEnumerable<KeyValue>? SetEnvVars { get; set; }

    [CliOption("--remove-env-vars")]
    public string[]? RemoveEnvVars { get; set; }

    [CliOption("--update-env-vars")]
    public IEnumerable<KeyValue>? UpdateEnvVars { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }
}