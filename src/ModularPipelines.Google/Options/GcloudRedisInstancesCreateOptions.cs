using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redis", "instances", "create")]
public record GcloudRedisInstancesCreateOptions(
[property: PositionalArgument] string Instance,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--alternative-zone")]
    public string? AlternativeZone { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--connect-mode")]
    public string? ConnectMode { get; set; }

    [CommandSwitch("--customer-managed-key")]
    public string? CustomerManagedKey { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--enable-auth")]
    public bool? EnableAuth { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--maintenance-window-day")]
    public string? MaintenanceWindowDay { get; set; }

    [CommandSwitch("--maintenance-window-hour")]
    public string? MaintenanceWindowHour { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--persistence-mode")]
    public string? PersistenceMode { get; set; }

    [CommandSwitch("--rdb-snapshot-period")]
    public string? RdbSnapshotPeriod { get; set; }

    [CommandSwitch("--rdb-snapshot-start-time")]
    public string? RdbSnapshotStartTime { get; set; }

    [CommandSwitch("--read-replicas-mode")]
    public string? ReadReplicasMode { get; set; }

    [CommandSwitch("--redis-config")]
    public IEnumerable<KeyValue>? RedisConfig { get; set; }

    [CommandSwitch("--redis-version")]
    public string? RedisVersion { get; set; }

    [CommandSwitch("--replica-count")]
    public string? ReplicaCount { get; set; }

    [CommandSwitch("--reserved-ip-range")]
    public string? ReservedIpRange { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--transit-encryption-mode")]
    public string? TransitEncryptionMode { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}