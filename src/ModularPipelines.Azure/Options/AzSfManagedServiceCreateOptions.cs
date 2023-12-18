using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "managed-service", "create")]
public record AzSfManagedServiceCreateOptions(
[property: CommandSwitch("--application")] string Application,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-type")] string ServiceType,
[property: CommandSwitch("--state")] string State
) : AzOptions
{
    [CommandSwitch("--activation-mode")]
    public string? ActivationMode { get; set; }

    [CommandSwitch("--default-move-cost")]
    public string? DefaultMoveCost { get; set; }

    [BooleanCommandSwitch("--has-persisted-state")]
    public bool? HasPersistedState { get; set; }

    [CommandSwitch("--high-key")]
    public string? HighKey { get; set; }

    [CommandSwitch("--instance-count")]
    public int? InstanceCount { get; set; }

    [CommandSwitch("--keep-duration")]
    public string? KeepDuration { get; set; }

    [CommandSwitch("--low-key")]
    public string? LowKey { get; set; }

    [CommandSwitch("--min-inst-pct")]
    public string? MinInstPct { get; set; }

    [CommandSwitch("--min-instance-count")]
    public int? MinInstanceCount { get; set; }

    [CommandSwitch("--min-replica")]
    public string? MinReplica { get; set; }

    [CommandSwitch("--partition-count")]
    public int? PartitionCount { get; set; }

    [CommandSwitch("--partition-names")]
    public string? PartitionNames { get; set; }

    [CommandSwitch("--partition-scheme")]
    public string? PartitionScheme { get; set; }

    [CommandSwitch("--placement-constraints")]
    public string? PlacementConstraints { get; set; }

    [CommandSwitch("--plcmt-time-limit")]
    public string? PlcmtTimeLimit { get; set; }

    [CommandSwitch("--quorum-loss-wait")]
    public string? QuorumLossWait { get; set; }

    [CommandSwitch("--replica-restart-wait")]
    public string? ReplicaRestartWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--target-replica")]
    public string? TargetReplica { get; set; }
}