using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sf", "managed-service", "update")]
public record AzSfManagedServiceUpdateOptions(
[property: CliOption("--application")] string Application,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--default-move-cost")]
    public string? DefaultMoveCost { get; set; }

    [CliOption("--instance-count")]
    public int? InstanceCount { get; set; }

    [CliOption("--keep-duration")]
    public string? KeepDuration { get; set; }

    [CliOption("--min-inst-pct")]
    public string? MinInstPct { get; set; }

    [CliOption("--min-instance-count")]
    public int? MinInstanceCount { get; set; }

    [CliOption("--min-replica")]
    public string? MinReplica { get; set; }

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