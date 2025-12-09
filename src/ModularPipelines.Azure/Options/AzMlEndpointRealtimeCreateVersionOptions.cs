using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "endpoint", "realtime", "create-version")]
public record AzMlEndpointRealtimeCreateVersionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--version-name")] string VersionName
) : AzOptions
{
    [CliOption("--add-property")]
    public string? AddProperty { get; set; }

    [CliOption("--add-tag")]
    public string? AddTag { get; set; }

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

    [CliOption("--cc")]
    public string? Cc { get; set; }

    [CliOption("--ccl")]
    public string? Ccl { get; set; }

    [CliOption("--cf")]
    public string? Cf { get; set; }

    [CliOption("--collect-model-data")]
    public string? CollectModelData { get; set; }

    [CliOption("--cvt")]
    public string? Cvt { get; set; }

    [CliOption("--dc")]
    public string? Dc { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--ed")]
    public string? Ed { get; set; }

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

    [CliOption("--is-default")]
    public string? IsDefault { get; set; }

    [CliOption("--max-request-wait-time")]
    public string? MaxRequestWaitTime { get; set; }

    [CliOption("--model")]
    public string? Model { get; set; }

    [CliOption("--model-metadata-file")]
    public string? ModelMetadataFile { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--nr")]
    public string? Nr { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--period-seconds")]
    public string? PeriodSeconds { get; set; }

    [CliOption("--replica-max-concurrent-requests")]
    public string? ReplicaMaxConcurrentRequests { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--scoring-timeout-ms")]
    public string? ScoringTimeoutMs { get; set; }

    [CliOption("--sd")]
    public string? Sd { get; set; }

    [CliOption("--st")]
    public string? St { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--timeout-seconds")]
    public string? TimeoutSeconds { get; set; }

    [CliOption("--tp")]
    public string? Tp { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CliOption("-v")]
    public string? V { get; set; }
}