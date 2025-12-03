using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netappfiles", "volume", "update")]
public record AzNetappfilesVolumeUpdateOptions : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliFlag("--backup-enabled")]
    public bool? BackupEnabled { get; set; }

    [CliOption("--backup-policy-id")]
    public string? BackupPolicyId { get; set; }

    [CliFlag("--cool-access")]
    public bool? CoolAccess { get; set; }

    [CliOption("--coolness-period")]
    public string? CoolnessPeriod { get; set; }

    [CliOption("--default-group-quota")]
    public string? DefaultGroupQuota { get; set; }

    [CliOption("--default-user-quota")]
    public string? DefaultUserQuota { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--is-def-quota-enabled")]
    public bool? IsDefQuotaEnabled { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--policy-enforced")]
    public bool? PolicyEnforced { get; set; }

    [CliOption("--pool-name")]
    public string? PoolName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--service-level")]
    public string? ServiceLevel { get; set; }

    [CliFlag("--snapshot-dir-visible")]
    public bool? SnapshotDirVisible { get; set; }

    [CliOption("--snapshot-policy-id")]
    public string? SnapshotPolicyId { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--throughput-mibps")]
    public string? ThroughputMibps { get; set; }

    [CliOption("--unix-permissions")]
    public string? UnixPermissions { get; set; }

    [CliOption("--usage-threshold")]
    public string? UsageThreshold { get; set; }
}