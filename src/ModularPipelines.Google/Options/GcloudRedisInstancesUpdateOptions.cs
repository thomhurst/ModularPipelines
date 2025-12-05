using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "instances", "update")]
public record GcloudRedisInstancesUpdateOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--enable-auth")]
    public bool? EnableAuth { get; set; }

    [CliOption("--maintenance-version")]
    public string? MaintenanceVersion { get; set; }

    [CliOption("--persistence-mode")]
    public string? PersistenceMode { get; set; }

    [CliOption("--rdb-snapshot-period")]
    public string? RdbSnapshotPeriod { get; set; }

    [CliOption("--rdb-snapshot-start-time")]
    public string? RdbSnapshotStartTime { get; set; }

    [CliOption("--read-replicas-mode")]
    public string? ReadReplicasMode { get; set; }

    [CliOption("--remove-redis-config")]
    public string[]? RemoveRedisConfig { get; set; }

    [CliOption("--replica-count")]
    public string? ReplicaCount { get; set; }

    [CliOption("--secondary-ip-range")]
    public string? SecondaryIpRange { get; set; }

    [CliOption("--size")]
    public string? Size { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliOption("--update-redis-config")]
    public IEnumerable<KeyValue>? UpdateRedisConfig { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliFlag("--maintenance-window-any")]
    public bool? MaintenanceWindowAny { get; set; }

    [CliOption("--maintenance-window-day")]
    public string? MaintenanceWindowDay { get; set; }

    [CliOption("--maintenance-window-hour")]
    public string? MaintenanceWindowHour { get; set; }
}