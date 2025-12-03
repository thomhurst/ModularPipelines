using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("run", "services", "update")]
public record GcloudRunServicesUpdateOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Namespace
) : GcloudOptions
{
    [CliOption("--args")]
    public string[]? Args { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--command")]
    public string[]? Command { get; set; }

    [CliOption("--concurrency")]
    public string? Concurrency { get; set; }

    [CliOption("--cpu")]
    public string? Cpu { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--ingress")]
    public string? Ingress { get; set; }

    [CliOption("--max-instances")]
    public string? MaxInstances { get; set; }

    [CliOption("--memory")]
    public string? Memory { get; set; }

    [CliOption("--min-instances")]
    public string? MinInstances { get; set; }

    [CliOption("--platform")]
    public string? Platform { get; set; }

    [CliOption("--port")]
    public string? Port { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliOption("--tag")]
    public string? Tag { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliFlag("--no-traffic")]
    public bool? NoTraffic { get; set; }

    [CliOption("--[no-]use-http2")]
    public string[]? NoUseHttp2 { get; set; }

    [CliOption("--breakglass")]
    public string? Breakglass { get; set; }

    [CliFlag("--clear-vpc-connector")]
    public bool? ClearVpcConnector { get; set; }

    [CliOption("--[no-]cpu-boost")]
    public string[]? NoCpuBoost { get; set; }

    [CliOption("--[no-]cpu-throttling")]
    public string[]? NoCpuThrottling { get; set; }

    [CliOption("--execution-environment")]
    public string? ExecutionEnvironment { get; set; }

    [CliFlag("gen1")]
    public bool? Gen1 { get; set; }

    [CliFlag("gen2")]
    public bool? Gen2 { get; set; }

    [CliOption("--revision-suffix")]
    public string? RevisionSuffix { get; set; }

    [CliOption("--[no-]session-affinity")]
    public string[]? NoSessionAffinity { get; set; }

    [CliOption("--vpc-connector")]
    public string? VpcConnector { get; set; }

    [CliOption("--vpc-egress")]
    public string? VpcEgress { get; set; }

    [CliFlag("all")]
    public bool? All { get; set; }

    [CliFlag("all-traffic")]
    public bool? AllTraffic { get; set; }

    [CliFlag("private-ranges-only")]
    public bool? PrivateRangesOnly { get; set; }

    [CliOption("--add-cloudsql-instances")]
    public string[]? AddCloudsqlInstances { get; set; }

    [CliFlag("--clear-cloudsql-instances")]
    public bool? ClearCloudsqlInstances { get; set; }

    [CliOption("--remove-cloudsql-instances")]
    public string[]? RemoveCloudsqlInstances { get; set; }

    [CliOption("--set-cloudsql-instances")]
    public string[]? SetCloudsqlInstances { get; set; }

    [CliOption("--add-custom-audiences")]
    public string[]? AddCustomAudiences { get; set; }

    [CliFlag("--clear-custom-audiences")]
    public bool? ClearCustomAudiences { get; set; }

    [CliOption("--remove-custom-audiences")]
    public string[]? RemoveCustomAudiences { get; set; }

    [CliOption("--set-custom-audiences")]
    public string[]? SetCustomAudiences { get; set; }

    [CliOption("--binary-authorization")]
    public string? BinaryAuthorization { get; set; }

    [CliFlag("--clear-binary-authorization")]
    public bool? ClearBinaryAuthorization { get; set; }

    [CliFlag("--clear-encryption-key-shutdown-hours")]
    public bool? ClearEncryptionKeyShutdownHours { get; set; }

    [CliOption("--encryption-key-shutdown-hours")]
    public string? EncryptionKeyShutdownHours { get; set; }

    [CliFlag("--clear-key")]
    public bool? ClearKey { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliFlag("--clear-post-key-revocation-action-type")]
    public bool? ClearPostKeyRevocationActionType { get; set; }

    [CliOption("--post-key-revocation-action-type")]
    public string? PostKeyRevocationActionType { get; set; }

    [CliFlag("prevent-new")]
    public bool? PreventNew { get; set; }

    [CliFlag("shut-down")]
    public bool? ShutDown { get; set; }

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

    [CliOption("--connectivity")]
    public string? Connectivity { get; set; }

    [CliFlag("external")]
    public bool? External { get; set; }

    [CliFlag("internal")]
    public bool? Internal { get; set; }

    [CliFlag("--clear-config-maps")]
    public bool? ClearConfigMaps { get; set; }

    [CliOption("--set-config-maps")]
    public IEnumerable<KeyValue>? SetConfigMaps { get; set; }

    [CliOption("--remove-config-maps")]
    public string[]? RemoveConfigMaps { get; set; }

    [CliOption("--update-config-maps")]
    public IEnumerable<KeyValue>? UpdateConfigMaps { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--cluster-location")]
    public string? ClusterLocation { get; set; }

    [CliOption("--context")]
    public string? Context { get; set; }

    [CliOption("--kubeconfig")]
    public string? Kubeconfig { get; set; }
}