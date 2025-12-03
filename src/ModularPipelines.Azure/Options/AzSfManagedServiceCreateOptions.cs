using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sf", "managed-service", "create")]
public record AzSfManagedServiceCreateOptions(
[property: CliOption("--application")] string Application,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-type")] string ServiceType,
[property: CliOption("--state")] string State
) : AzOptions
{
    [CliOption("--activation-mode")]
    public string? ActivationMode { get; set; }

    [CliOption("--default-move-cost")]
    public string? DefaultMoveCost { get; set; }

    [CliFlag("--has-persisted-state")]
    public bool? HasPersistedState { get; set; }

    [CliOption("--high-key")]
    public string? HighKey { get; set; }

    [CliOption("--instance-count")]
    public int? InstanceCount { get; set; }

    [CliOption("--keep-duration")]
    public string? KeepDuration { get; set; }

    [CliOption("--low-key")]
    public string? LowKey { get; set; }

    [CliOption("--min-inst-pct")]
    public string? MinInstPct { get; set; }

    [CliOption("--min-instance-count")]
    public int? MinInstanceCount { get; set; }

    [CliOption("--min-replica")]
    public string? MinReplica { get; set; }

    [CliOption("--partition-count")]
    public int? PartitionCount { get; set; }

    [CliOption("--partition-names")]
    public string? PartitionNames { get; set; }

    [CliOption("--partition-scheme")]
    public string? PartitionScheme { get; set; }

    [CliOption("--placement-constraints")]
    public string? PlacementConstraints { get; set; }

    [CliOption("--plcmt-time-limit")]
    public string? PlcmtTimeLimit { get; set; }

    [CliOption("--quorum-loss-wait")]
    public string? QuorumLossWait { get; set; }

    [CliOption("--replica-restart-wait")]
    public string? ReplicaRestartWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--target-replica")]
    public string? TargetReplica { get; set; }
}