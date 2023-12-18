using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "volume", "update")]
public record AzNetappfilesVolumeUpdateOptions : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [BooleanCommandSwitch("--backup-enabled")]
    public bool? BackupEnabled { get; set; }

    [CommandSwitch("--backup-policy-id")]
    public string? BackupPolicyId { get; set; }

    [BooleanCommandSwitch("--cool-access")]
    public bool? CoolAccess { get; set; }

    [CommandSwitch("--coolness-period")]
    public string? CoolnessPeriod { get; set; }

    [CommandSwitch("--default-group-quota")]
    public string? DefaultGroupQuota { get; set; }

    [CommandSwitch("--default-user-quota")]
    public string? DefaultUserQuota { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--is-def-quota-enabled")]
    public bool? IsDefQuotaEnabled { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--policy-enforced")]
    public bool? PolicyEnforced { get; set; }

    [CommandSwitch("--pool-name")]
    public string? PoolName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--service-level")]
    public string? ServiceLevel { get; set; }

    [BooleanCommandSwitch("--snapshot-dir-visible")]
    public bool? SnapshotDirVisible { get; set; }

    [CommandSwitch("--snapshot-policy-id")]
    public string? SnapshotPolicyId { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--throughput-mibps")]
    public string? ThroughputMibps { get; set; }

    [CommandSwitch("--unix-permissions")]
    public string? UnixPermissions { get; set; }

    [CommandSwitch("--usage-threshold")]
    public string? UsageThreshold { get; set; }
}