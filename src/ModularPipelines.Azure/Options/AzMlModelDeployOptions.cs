using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "model", "deploy")]
public record AzMlModelDeployOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--ae")]
    public string? Ae { get; set; }

    [CliOption("--ai")]
    public string? Ai { get; set; }

    [CliOption("--ar")]
    public string? Ar { get; set; }

    [CliOption("--as")]
    public string? As { get; set; }

    [CliOption("--at")]
    public string? At { get; set; }

    [CliOption("--autoscale-max-replicas")]
    public string? AutoscaleMaxReplicas { get; set; }

    [CliOption("--autoscale-min-replicas")]
    public string? AutoscaleMinReplicas { get; set; }

    [CliOption("--base-image")]
    public string? BaseImage { get; set; }

    [CliOption("--base-image-registry")]
    public string? BaseImageRegistry { get; set; }

    [CliOption("--cc")]
    public string? Cc { get; set; }

    [CliOption("--ccl")]
    public string? Ccl { get; set; }

    [CliOption("--cf")]
    public string? Cf { get; set; }

    [CliOption("--collect-model-data")]
    public string? CollectModelData { get; set; }

    [CliOption("--compute-target")]
    public string? ComputeTarget { get; set; }

    [CliOption("--compute-type")]
    public string? ComputeType { get; set; }

    [CliOption("--cuda-version")]
    public string? CudaVersion { get; set; }

    [CliOption("--dc")]
    public string? Dc { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--dn")]
    public string? Dn { get; set; }

    [CliOption("--ds")]
    public string? Ds { get; set; }

    [CliOption("--ed")]
    public string? Ed { get; set; }

    [CliOption("--eg")]
    public string? Eg { get; set; }

    [CliOption("--entry-script")]
    public string? EntryScript { get; set; }

    [CliOption("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CliOption("--environment-version")]
    public string? EnvironmentVersion { get; set; }

    [CliOption("--failure-threshold")]
    public string? FailureThreshold { get; set; }

    [CliOption("--gb")]
    public string? Gb { get; set; }

    [CliOption("--gbl")]
    public string? Gbl { get; set; }

    [CliOption("--gc")]
    public string? Gc { get; set; }

    [CliOption("--ic")]
    public string? Ic { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--key-name")]
    public string? KeyName { get; set; }

    [CliOption("--key-version")]
    public string? KeyVersion { get; set; }

    [CliOption("--kp")]
    public string? Kp { get; set; }

    [CliOption("--ks")]
    public string? Ks { get; set; }

    [CliOption("--lo")]
    public string? Lo { get; set; }

    [CliOption("--max-request-wait-time")]
    public string? MaxRequestWaitTime { get; set; }

    [CliOption("--model")]
    public string? Model { get; set; }

    [CliOption("--model-metadata-file")]
    public string? ModelMetadataFile { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--nr")]
    public string? Nr { get; set; }

    [CliOption("--overwrite")]
    public string? Overwrite { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--period-seconds")]
    public string? PeriodSeconds { get; set; }

    [CliOption("--pi")]
    public string? Pi { get; set; }

    [CliOption("--po")]
    public string? Po { get; set; }

    [CliOption("--property")]
    public string? Property { get; set; }

    [CliOption("--replica-max-concurrent-requests")]
    public string? ReplicaMaxConcurrentRequests { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rt")]
    public string? Rt { get; set; }

    [CliOption("--sc")]
    public string? Sc { get; set; }

    [CliOption("--scoring-timeout-ms")]
    public string? ScoringTimeoutMs { get; set; }

    [CliOption("--sd")]
    public string? Sd { get; set; }

    [CliOption("--se")]
    public string? Se { get; set; }

    [CliOption("--sk")]
    public string? Sk { get; set; }

    [CliOption("--sp")]
    public string? Sp { get; set; }

    [CliOption("--st")]
    public string? St { get; set; }

    [CliOption("--subnet-name")]
    public string? SubnetName { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--tag")]
    public string? Tag { get; set; }

    [CliOption("--timeout-seconds")]
    public string? TimeoutSeconds { get; set; }

    [CliFlag("--token-auth-enabled")]
    public bool? TokenAuthEnabled { get; set; }

    [CliOption("--tp")]
    public string? Tp { get; set; }

    [CliOption("--vault-base-url")]
    public string? VaultBaseUrl { get; set; }

    [CliOption("--version-name")]
    public string? VersionName { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CliOption("-v")]
    public string? V { get; set; }
}