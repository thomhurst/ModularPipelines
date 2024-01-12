using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functions", "deploy")]
public record GcloudFunctionsDeployOptions(
[property: PositionalArgument] string Name,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--[no-]allow-unauthenticated")]
    public string[]? NoAllowUnauthenticated { get; set; }

    [CommandSwitch("--concurrency")]
    public string? Concurrency { get; set; }

    [CommandSwitch("--docker-registry")]
    public string? DockerRegistry { get; set; }

    [CommandSwitch("--egress-settings")]
    public string? EgressSettings { get; set; }

    [CommandSwitch("--entry-point")]
    public string? EntryPoint { get; set; }

    [BooleanCommandSwitch("--gen2")]
    public bool? Gen2 { get; set; }

    [CommandSwitch("--ignore-file")]
    public string? IgnoreFile { get; set; }

    [CommandSwitch("--ingress-settings")]
    public string? IngressSettings { get; set; }

    [BooleanCommandSwitch("--retry")]
    public bool? Retry { get; set; }

    [CommandSwitch("--run-service-account")]
    public string? RunServiceAccount { get; set; }

    [CommandSwitch("--runtime")]
    public string? Runtime { get; set; }

    [CommandSwitch("--runtime-update-policy")]
    public string? RuntimeUpdatePolicy { get; set; }

    [CommandSwitch("--security-level")]
    public string? SecurityLevel { get; set; }

    [BooleanCommandSwitch("--serve-all-traffic-latest-revision")]
    public bool? ServeAllTrafficLatestRevision { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--stage-bucket")]
    public string? StageBucket { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--trigger-location")]
    public string? TriggerLocation { get; set; }

    [CommandSwitch("--trigger-service-account")]
    public string? TriggerServiceAccount { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CommandSwitch("--build-env-vars-file")]
    public string? BuildEnvVarsFile { get; set; }

    [BooleanCommandSwitch("--clear-build-env-vars")]
    public bool? ClearBuildEnvVars { get; set; }

    [CommandSwitch("--set-build-env-vars")]
    public IEnumerable<KeyValue>? SetBuildEnvVars { get; set; }

    [CommandSwitch("--remove-build-env-vars")]
    public string[]? RemoveBuildEnvVars { get; set; }

    [CommandSwitch("--update-build-env-vars")]
    public IEnumerable<KeyValue>? UpdateBuildEnvVars { get; set; }

    [CommandSwitch("--build-worker-pool")]
    public string? BuildWorkerPool { get; set; }

    [BooleanCommandSwitch("--clear-build-worker-pool")]
    public bool? ClearBuildWorkerPool { get; set; }

    [BooleanCommandSwitch("--clear-docker-repository")]
    public bool? ClearDockerRepository { get; set; }

    [CommandSwitch("--docker-repository")]
    public string? DockerRepository { get; set; }

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

    [BooleanCommandSwitch("--clear-kms-key")]
    public bool? ClearKmsKey { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [BooleanCommandSwitch("--clear-max-instances")]
    public bool? ClearMaxInstances { get; set; }

    [CommandSwitch("--max-instances")]
    public string? MaxInstances { get; set; }

    [BooleanCommandSwitch("--clear-min-instances")]
    public bool? ClearMinInstances { get; set; }

    [CommandSwitch("--min-instances")]
    public string? MinInstances { get; set; }

    [BooleanCommandSwitch("--clear-secrets")]
    public bool? ClearSecrets { get; set; }

    [CommandSwitch("--set-secrets")]
    public string[]? SetSecrets { get; set; }

    [CommandSwitch("--remove-secrets")]
    public string[]? RemoveSecrets { get; set; }

    [CommandSwitch("--update-secrets")]
    public string[]? UpdateSecrets { get; set; }

    [BooleanCommandSwitch("--clear-vpc-connector")]
    public bool? ClearVpcConnector { get; set; }

    [CommandSwitch("--vpc-connector")]
    public string? VpcConnector { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [CommandSwitch("--cpu")]
    public string? Cpu { get; set; }

    [CommandSwitch("--trigger-bucket")]
    public string? TriggerBucket { get; set; }

    [BooleanCommandSwitch("--trigger-http")]
    public bool? TriggerHttp { get; set; }

    [CommandSwitch("--trigger-topic")]
    public string? TriggerTopic { get; set; }

    [CommandSwitch("--trigger-event")]
    public string? TriggerEvent { get; set; }

    [CommandSwitch("--trigger-resource")]
    public string? TriggerResource { get; set; }

    [CommandSwitch("--trigger-event-filters")]
    public string[]? TriggerEventFilters { get; set; }

    [CommandSwitch("--trigger-event-filters-path-pattern")]
    public string[]? TriggerEventFiltersPathPattern { get; set; }
}