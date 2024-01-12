using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redis", "instances", "update")]
public record GcloudRedisInstancesUpdateOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--enable-auth")]
    public bool? EnableAuth { get; set; }

    [CommandSwitch("--maintenance-version")]
    public string? MaintenanceVersion { get; set; }

    [CommandSwitch("--persistence-mode")]
    public string? PersistenceMode { get; set; }

    [CommandSwitch("--rdb-snapshot-period")]
    public string? RdbSnapshotPeriod { get; set; }

    [CommandSwitch("--rdb-snapshot-start-time")]
    public string? RdbSnapshotStartTime { get; set; }

    [CommandSwitch("--read-replicas-mode")]
    public string? ReadReplicasMode { get; set; }

    [CommandSwitch("--remove-redis-config")]
    public string[]? RemoveRedisConfig { get; set; }

    [CommandSwitch("--replica-count")]
    public string? ReplicaCount { get; set; }

    [CommandSwitch("--secondary-ip-range")]
    public string? SecondaryIpRange { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CommandSwitch("--update-redis-config")]
    public IEnumerable<KeyValue>? UpdateRedisConfig { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [BooleanCommandSwitch("--maintenance-window-any")]
    public bool? MaintenanceWindowAny { get; set; }

    [CommandSwitch("--maintenance-window-day")]
    public string? MaintenanceWindowDay { get; set; }

    [CommandSwitch("--maintenance-window-hour")]
    public string? MaintenanceWindowHour { get; set; }
}