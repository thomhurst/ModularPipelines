using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "managed-service", "show")]
public record AzSfManagedServiceShowOptions(
[property: CommandSwitch("--application")] string Application,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--default-move-cost")]
    public string? DefaultMoveCost { get; set; }

    [CommandSwitch("--instance-count")]
    public int? InstanceCount { get; set; }

    [CommandSwitch("--keep-duration")]
    public string? KeepDuration { get; set; }

    [CommandSwitch("--min-inst-pct")]
    public string? MinInstPct { get; set; }

    [CommandSwitch("--min-instance-count")]
    public int? MinInstanceCount { get; set; }

    [CommandSwitch("--min-replica")]
    public string? MinReplica { get; set; }

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