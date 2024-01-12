using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("run", "deploy")]
public record GcloudRunDeployOptions(
[property: PositionalArgument] string Service,
[property: PositionalArgument] string Namespace
) : GcloudOptions
{
    [CommandSwitch("--args")]
    public string[]? Args { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--command")]
    public string[]? Command { get; set; }

    [CommandSwitch("--concurrency")]
    public string? Concurrency { get; set; }

    [CommandSwitch("--cpu")]
    public string? Cpu { get; set; }

    [CommandSwitch("--ingress")]
    public string? Ingress { get; set; }

    [CommandSwitch("--max-instances")]
    public string? MaxInstances { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [CommandSwitch("--min-instances")]
    public string? MinInstances { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [CommandSwitch("--port")]
    public string? Port { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [BooleanCommandSwitch("--no-traffic")]
    public bool? NoTraffic { get; set; }

    [CommandSwitch("--[no-]use-http2")]
    public string[]? NoUseHttp2 { get; set; }

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

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-secrets")]
    public bool? ClearSecrets { get; set; }

    [CommandSwitch("--set-secrets")]
    public IEnumerable<KeyValue>? SetSecrets { get; set; }

    [CommandSwitch("--remove-secrets")]
    public string[]? RemoveSecrets { get; set; }

    [CommandSwitch("--update-secrets")]
    public IEnumerable<KeyValue>? UpdateSecrets { get; set; }

    [CommandSwitch("--connectivity")]
    public string? Connectivity { get; set; }

    [BooleanCommandSwitch("external")]
    public bool? External { get; set; }

    [BooleanCommandSwitch("internal")]
    public bool? Internal { get; set; }

    [BooleanCommandSwitch("--clear-config-maps")]
    public bool? ClearConfigMaps { get; set; }

    [CommandSwitch("--set-config-maps")]
    public IEnumerable<KeyValue>? SetConfigMaps { get; set; }

    [CommandSwitch("--remove-config-maps")]
    public string[]? RemoveConfigMaps { get; set; }

    [CommandSwitch("--update-config-maps")]
    public IEnumerable<KeyValue>? UpdateConfigMaps { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--[no-]allow-unauthenticated")]
    public string[]? NoAllowUnauthenticated { get; set; }

    [CommandSwitch("--breakglass")]
    public string? Breakglass { get; set; }

    [BooleanCommandSwitch("--clear-vpc-connector")]
    public bool? ClearVpcConnector { get; set; }

    [CommandSwitch("--[no-]cpu-boost")]
    public string[]? NoCpuBoost { get; set; }

    [CommandSwitch("--[no-]cpu-throttling")]
    public string[]? NoCpuThrottling { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--execution-environment")]
    public string? ExecutionEnvironment { get; set; }

    [BooleanCommandSwitch("gen1")]
    public bool? Gen1 { get; set; }

    [BooleanCommandSwitch("gen2")]
    public bool? Gen2 { get; set; }

    [CommandSwitch("--revision-suffix")]
    public string? RevisionSuffix { get; set; }

    [CommandSwitch("--[no-]session-affinity")]
    public string[]? NoSessionAffinity { get; set; }

    [CommandSwitch("--vpc-connector")]
    public string? VpcConnector { get; set; }

    [CommandSwitch("--vpc-egress")]
    public string? VpcEgress { get; set; }

    [BooleanCommandSwitch("all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("all-traffic")]
    public bool? AllTraffic { get; set; }

    [BooleanCommandSwitch("private-ranges-only")]
    public bool? PrivateRangesOnly { get; set; }

    [CommandSwitch("--add-cloudsql-instances")]
    public string[]? AddCloudsqlInstances { get; set; }

    [BooleanCommandSwitch("--clear-cloudsql-instances")]
    public bool? ClearCloudsqlInstances { get; set; }

    [CommandSwitch("--remove-cloudsql-instances")]
    public string[]? RemoveCloudsqlInstances { get; set; }

    [CommandSwitch("--set-cloudsql-instances")]
    public string[]? SetCloudsqlInstances { get; set; }

    [CommandSwitch("--add-custom-audiences")]
    public string[]? AddCustomAudiences { get; set; }

    [BooleanCommandSwitch("--clear-custom-audiences")]
    public bool? ClearCustomAudiences { get; set; }

    [CommandSwitch("--remove-custom-audiences")]
    public string[]? RemoveCustomAudiences { get; set; }

    [CommandSwitch("--set-custom-audiences")]
    public string[]? SetCustomAudiences { get; set; }

    [CommandSwitch("--binary-authorization")]
    public string? BinaryAuthorization { get; set; }

    [BooleanCommandSwitch("--clear-binary-authorization")]
    public bool? ClearBinaryAuthorization { get; set; }

    [BooleanCommandSwitch("--clear-encryption-key-shutdown-hours")]
    public bool? ClearEncryptionKeyShutdownHours { get; set; }

    [CommandSwitch("--encryption-key-shutdown-hours")]
    public string? EncryptionKeyShutdownHours { get; set; }

    [BooleanCommandSwitch("--clear-key")]
    public bool? ClearKey { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [BooleanCommandSwitch("--clear-post-key-revocation-action-type")]
    public bool? ClearPostKeyRevocationActionType { get; set; }

    [CommandSwitch("--post-key-revocation-action-type")]
    public string? PostKeyRevocationActionType { get; set; }

    [BooleanCommandSwitch("prevent-new")]
    public bool? PreventNew { get; set; }

    [BooleanCommandSwitch("shut-down")]
    public bool? ShutDown { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [CommandSwitch("--cluster-location")]
    public string? ClusterLocation { get; set; }

    [CommandSwitch("--context")]
    public string? Context { get; set; }

    [CommandSwitch("--kubeconfig")]
    public string? Kubeconfig { get; set; }
}