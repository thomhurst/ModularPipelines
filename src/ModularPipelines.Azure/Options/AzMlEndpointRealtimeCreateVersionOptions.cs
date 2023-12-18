using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "endpoint", "realtime", "create-version")]
public record AzMlEndpointRealtimeCreateVersionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--version-name")] string VersionName
) : AzOptions
{
    [CommandSwitch("--add-property")]
    public string? AddProperty { get; set; }

    [CommandSwitch("--add-tag")]
    public string? AddTag { get; set; }

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

    [CommandSwitch("--cc")]
    public string? Cc { get; set; }

    [CommandSwitch("--ccl")]
    public string? Ccl { get; set; }

    [CommandSwitch("--cf")]
    public string? Cf { get; set; }

    [CommandSwitch("--collect-model-data")]
    public string? CollectModelData { get; set; }

    [CommandSwitch("--cvt")]
    public string? Cvt { get; set; }

    [CommandSwitch("--dc")]
    public string? Dc { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--ed")]
    public string? Ed { get; set; }

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

    [CommandSwitch("--is-default")]
    public string? IsDefault { get; set; }

    [CommandSwitch("--max-request-wait-time")]
    public string? MaxRequestWaitTime { get; set; }

    [CommandSwitch("--model")]
    public string? Model { get; set; }

    [CommandSwitch("--model-metadata-file")]
    public string? ModelMetadataFile { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--nr")]
    public string? Nr { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--period-seconds")]
    public string? PeriodSeconds { get; set; }

    [CommandSwitch("--replica-max-concurrent-requests")]
    public string? ReplicaMaxConcurrentRequests { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--scoring-timeout-ms")]
    public string? ScoringTimeoutMs { get; set; }

    [CommandSwitch("--sd")]
    public string? Sd { get; set; }

    [CommandSwitch("--st")]
    public string? St { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--timeout-seconds")]
    public string? TimeoutSeconds { get; set; }

    [CommandSwitch("--tp")]
    public string? Tp { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CommandSwitch("-v")]
    public string? V { get; set; }
}