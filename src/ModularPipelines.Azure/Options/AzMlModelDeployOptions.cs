using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "model", "deploy")]
public record AzMlModelDeployOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--ae")]
    public string? Ae { get; set; }

    [CommandSwitch("--ai")]
    public string? Ai { get; set; }

    [CommandSwitch("--ar")]
    public string? Ar { get; set; }

    [CommandSwitch("--as")]
    public string? As { get; set; }

    [CommandSwitch("--at")]
    public string? At { get; set; }

    [CommandSwitch("--autoscale-max-replicas")]
    public string? AutoscaleMaxReplicas { get; set; }

    [CommandSwitch("--autoscale-min-replicas")]
    public string? AutoscaleMinReplicas { get; set; }

    [CommandSwitch("--base-image")]
    public string? BaseImage { get; set; }

    [CommandSwitch("--base-image-registry")]
    public string? BaseImageRegistry { get; set; }

    [CommandSwitch("--cc")]
    public string? Cc { get; set; }

    [CommandSwitch("--ccl")]
    public string? Ccl { get; set; }

    [CommandSwitch("--cf")]
    public string? Cf { get; set; }

    [CommandSwitch("--collect-model-data")]
    public string? CollectModelData { get; set; }

    [CommandSwitch("--compute-target")]
    public string? ComputeTarget { get; set; }

    [CommandSwitch("--compute-type")]
    public string? ComputeType { get; set; }

    [CommandSwitch("--cuda-version")]
    public string? CudaVersion { get; set; }

    [CommandSwitch("--dc")]
    public string? Dc { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--dn")]
    public string? Dn { get; set; }

    [CommandSwitch("--ds")]
    public string? Ds { get; set; }

    [CommandSwitch("--ed")]
    public string? Ed { get; set; }

    [CommandSwitch("--eg")]
    public string? Eg { get; set; }

    [CommandSwitch("--entry-script")]
    public string? EntryScript { get; set; }

    [CommandSwitch("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CommandSwitch("--environment-version")]
    public string? EnvironmentVersion { get; set; }

    [CommandSwitch("--failure-threshold")]
    public string? FailureThreshold { get; set; }

    [CommandSwitch("--gb")]
    public string? Gb { get; set; }

    [CommandSwitch("--gbl")]
    public string? Gbl { get; set; }

    [CommandSwitch("--gc")]
    public string? Gc { get; set; }

    [CommandSwitch("--ic")]
    public string? Ic { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--key-name")]
    public string? KeyName { get; set; }

    [CommandSwitch("--key-version")]
    public string? KeyVersion { get; set; }

    [CommandSwitch("--kp")]
    public string? Kp { get; set; }

    [CommandSwitch("--ks")]
    public string? Ks { get; set; }

    [CommandSwitch("--lo")]
    public string? Lo { get; set; }

    [CommandSwitch("--max-request-wait-time")]
    public string? MaxRequestWaitTime { get; set; }

    [CommandSwitch("--model")]
    public string? Model { get; set; }

    [CommandSwitch("--model-metadata-file")]
    public string? ModelMetadataFile { get; set; }

    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--nr")]
    public string? Nr { get; set; }

    [CommandSwitch("--overwrite")]
    public string? Overwrite { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--period-seconds")]
    public string? PeriodSeconds { get; set; }

    [CommandSwitch("--pi")]
    public string? Pi { get; set; }

    [CommandSwitch("--po")]
    public string? Po { get; set; }

    [CommandSwitch("--property")]
    public string? Property { get; set; }

    [CommandSwitch("--replica-max-concurrent-requests")]
    public string? ReplicaMaxConcurrentRequests { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rt")]
    public string? Rt { get; set; }

    [CommandSwitch("--sc")]
    public string? Sc { get; set; }

    [CommandSwitch("--scoring-timeout-ms")]
    public string? ScoringTimeoutMs { get; set; }

    [CommandSwitch("--sd")]
    public string? Sd { get; set; }

    [CommandSwitch("--se")]
    public string? Se { get; set; }

    [CommandSwitch("--sk")]
    public string? Sk { get; set; }

    [CommandSwitch("--sp")]
    public string? Sp { get; set; }

    [CommandSwitch("--st")]
    public string? St { get; set; }

    [CommandSwitch("--subnet-name")]
    public string? SubnetName { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }

    [CommandSwitch("--timeout-seconds")]
    public string? TimeoutSeconds { get; set; }

    [BooleanCommandSwitch("--token-auth-enabled")]
    public bool? TokenAuthEnabled { get; set; }

    [CommandSwitch("--tp")]
    public string? Tp { get; set; }

    [CommandSwitch("--vault-base-url")]
    public string? VaultBaseUrl { get; set; }

    [CommandSwitch("--version-name")]
    public string? VersionName { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CommandSwitch("-v")]
    public string? V { get; set; }
}

