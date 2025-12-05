using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("run", "jobs", "update")]
public record GcloudRunJobsUpdateOptions(
[property: CliArgument] string Job
) : GcloudOptions
{
    [CliOption("--args")]
    public string[]? Args { get; set; }

    [CliOption("--breakglass")]
    public string? Breakglass { get; set; }

    [CliFlag("--clear-vpc-connector")]
    public bool? ClearVpcConnector { get; set; }

    [CliOption("--command")]
    public string[]? Command { get; set; }

    [CliOption("--cpu")]
    public string? Cpu { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

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

    [CliOption("--task-timeout")]
    public string? TaskTimeout { get; set; }

    [CliOption("--tasks")]
    public string? Tasks { get; set; }

    [CliOption("--vpc-connector")]
    public string? VpcConnector { get; set; }

    [CliOption("--vpc-egress")]
    public string? VpcEgress { get; set; }

    [CliOption("--add-cloudsql-instances")]
    public string[]? AddCloudsqlInstances { get; set; }

    [CliFlag("--clear-cloudsql-instances")]
    public bool? ClearCloudsqlInstances { get; set; }

    [CliOption("--remove-cloudsql-instances")]
    public string[]? RemoveCloudsqlInstances { get; set; }

    [CliOption("--set-cloudsql-instances")]
    public string[]? SetCloudsqlInstances { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--execute-now")]
    public bool? ExecuteNow { get; set; }

    [CliFlag("--wait")]
    public bool? Wait { get; set; }

    [CliOption("--binary-authorization")]
    public string? BinaryAuthorization { get; set; }

    [CliFlag("--clear-binary-authorization")]
    public bool? ClearBinaryAuthorization { get; set; }

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

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-secrets")]
    public bool? ClearSecrets { get; set; }

    [CliOption("--set-secrets")]
    public IEnumerable<KeyValue>? SetSecrets { get; set; }

    [CliOption("--remove-secrets")]
    public string[]? RemoveSecrets { get; set; }

    [CliOption("--update-secrets")]
    public IEnumerable<KeyValue>? UpdateSecrets { get; set; }
}