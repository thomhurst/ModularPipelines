using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("functions", "deploy")]
public record GcloudFunctionsDeployOptions(
[property: CliArgument] string Name,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--[no-]allow-unauthenticated")]
    public string[]? NoAllowUnauthenticated { get; set; }

    [CliOption("--concurrency")]
    public string? Concurrency { get; set; }

    [CliOption("--docker-registry")]
    public string? DockerRegistry { get; set; }

    [CliOption("--egress-settings")]
    public string? EgressSettings { get; set; }

    [CliOption("--entry-point")]
    public string? EntryPoint { get; set; }

    [CliFlag("--gen2")]
    public bool? Gen2 { get; set; }

    [CliOption("--ignore-file")]
    public string? IgnoreFile { get; set; }

    [CliOption("--ingress-settings")]
    public string? IngressSettings { get; set; }

    [CliFlag("--retry")]
    public bool? Retry { get; set; }

    [CliOption("--run-service-account")]
    public string? RunServiceAccount { get; set; }

    [CliOption("--runtime")]
    public string? Runtime { get; set; }

    [CliOption("--runtime-update-policy")]
    public string? RuntimeUpdatePolicy { get; set; }

    [CliOption("--security-level")]
    public string? SecurityLevel { get; set; }

    [CliFlag("--serve-all-traffic-latest-revision")]
    public bool? ServeAllTrafficLatestRevision { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--stage-bucket")]
    public string? StageBucket { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--trigger-location")]
    public string? TriggerLocation { get; set; }

    [CliOption("--trigger-service-account")]
    public string? TriggerServiceAccount { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliOption("--build-env-vars-file")]
    public string? BuildEnvVarsFile { get; set; }

    [CliFlag("--clear-build-env-vars")]
    public bool? ClearBuildEnvVars { get; set; }

    [CliOption("--set-build-env-vars")]
    public IEnumerable<KeyValue>? SetBuildEnvVars { get; set; }

    [CliOption("--remove-build-env-vars")]
    public string[]? RemoveBuildEnvVars { get; set; }

    [CliOption("--update-build-env-vars")]
    public IEnumerable<KeyValue>? UpdateBuildEnvVars { get; set; }

    [CliOption("--build-worker-pool")]
    public string? BuildWorkerPool { get; set; }

    [CliFlag("--clear-build-worker-pool")]
    public bool? ClearBuildWorkerPool { get; set; }

    [CliFlag("--clear-docker-repository")]
    public bool? ClearDockerRepository { get; set; }

    [CliOption("--docker-repository")]
    public string? DockerRepository { get; set; }

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

    [CliFlag("--clear-kms-key")]
    public bool? ClearKmsKey { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliFlag("--clear-max-instances")]
    public bool? ClearMaxInstances { get; set; }

    [CliOption("--max-instances")]
    public string? MaxInstances { get; set; }

    [CliFlag("--clear-min-instances")]
    public bool? ClearMinInstances { get; set; }

    [CliOption("--min-instances")]
    public string? MinInstances { get; set; }

    [CliFlag("--clear-secrets")]
    public bool? ClearSecrets { get; set; }

    [CliOption("--set-secrets")]
    public string[]? SetSecrets { get; set; }

    [CliOption("--remove-secrets")]
    public string[]? RemoveSecrets { get; set; }

    [CliOption("--update-secrets")]
    public string[]? UpdateSecrets { get; set; }

    [CliFlag("--clear-vpc-connector")]
    public bool? ClearVpcConnector { get; set; }

    [CliOption("--vpc-connector")]
    public string? VpcConnector { get; set; }

    [CliOption("--memory")]
    public string? Memory { get; set; }

    [CliOption("--cpu")]
    public string? Cpu { get; set; }

    [CliOption("--trigger-bucket")]
    public string? TriggerBucket { get; set; }

    [CliFlag("--trigger-http")]
    public bool? TriggerHttp { get; set; }

    [CliOption("--trigger-topic")]
    public string? TriggerTopic { get; set; }

    [CliOption("--trigger-event")]
    public string? TriggerEvent { get; set; }

    [CliOption("--trigger-resource")]
    public string? TriggerResource { get; set; }

    [CliOption("--trigger-event-filters")]
    public string[]? TriggerEventFilters { get; set; }

    [CliOption("--trigger-event-filters-path-pattern")]
    public string[]? TriggerEventFiltersPathPattern { get; set; }
}