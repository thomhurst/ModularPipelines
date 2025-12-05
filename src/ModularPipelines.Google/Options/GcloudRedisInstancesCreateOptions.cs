using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "instances", "create")]
public record GcloudRedisInstancesCreateOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--alternative-zone")]
    public string? AlternativeZone { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--connect-mode")]
    public string? ConnectMode { get; set; }

    [CliOption("--customer-managed-key")]
    public string? CustomerManagedKey { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--enable-auth")]
    public bool? EnableAuth { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--maintenance-window-day")]
    public string? MaintenanceWindowDay { get; set; }

    [CliOption("--maintenance-window-hour")]
    public string? MaintenanceWindowHour { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--persistence-mode")]
    public string? PersistenceMode { get; set; }

    [CliOption("--rdb-snapshot-period")]
    public string? RdbSnapshotPeriod { get; set; }

    [CliOption("--rdb-snapshot-start-time")]
    public string? RdbSnapshotStartTime { get; set; }

    [CliOption("--read-replicas-mode")]
    public string? ReadReplicasMode { get; set; }

    [CliOption("--redis-config")]
    public IEnumerable<KeyValue>? RedisConfig { get; set; }

    [CliOption("--redis-version")]
    public string? RedisVersion { get; set; }

    [CliOption("--replica-count")]
    public string? ReplicaCount { get; set; }

    [CliOption("--reserved-ip-range")]
    public string? ReservedIpRange { get; set; }

    [CliOption("--size")]
    public string? Size { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--transit-encryption-mode")]
    public string? TransitEncryptionMode { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}