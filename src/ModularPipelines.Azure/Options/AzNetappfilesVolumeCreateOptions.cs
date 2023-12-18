using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "volume", "create")]
public record AzNetappfilesVolumeCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--file-path")] string FilePath,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--pool-name")] string PoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--usage-threshold")] string UsageThreshold,
[property: CommandSwitch("--vnet")] string Vnet
) : AzOptions
{
    [BooleanCommandSwitch("--allowed-clients")]
    public bool? AllowedClients { get; set; }

    [CommandSwitch("--avs-data-store")]
    public string? AvsDataStore { get; set; }

    [BooleanCommandSwitch("--backup-enabled")]
    public bool? BackupEnabled { get; set; }

    [CommandSwitch("--backup-id")]
    public string? BackupId { get; set; }

    [CommandSwitch("--backup-policy-id")]
    public string? BackupPolicyId { get; set; }

    [CommandSwitch("--chown-mode")]
    public string? ChownMode { get; set; }

    [BooleanCommandSwitch("--cifs")]
    public bool? Cifs { get; set; }

    [BooleanCommandSwitch("--cool-access")]
    public bool? CoolAccess { get; set; }

    [CommandSwitch("--coolness-period")]
    public string? CoolnessPeriod { get; set; }

    [CommandSwitch("--default-group-quota")]
    public string? DefaultGroupQuota { get; set; }

    [CommandSwitch("--default-user-quota")]
    public string? DefaultUserQuota { get; set; }

    [BooleanCommandSwitch("--delete-base-snapshot")]
    public bool? DeleteBaseSnapshot { get; set; }

    [CommandSwitch("--enable-subvolumes")]
    public string? EnableSubvolumes { get; set; }

    [CommandSwitch("--encryption-key-source")]
    public string? EncryptionKeySource { get; set; }

    [CommandSwitch("--endpoint-type")]
    public string? EndpointType { get; set; }

    [BooleanCommandSwitch("--has-root-access")]
    public bool? HasRootAccess { get; set; }

    [BooleanCommandSwitch("--is-def-quota-enabled")]
    public bool? IsDefQuotaEnabled { get; set; }

    [BooleanCommandSwitch("--is-large-volume")]
    public bool? IsLargeVolume { get; set; }

    [BooleanCommandSwitch("--kerberos-enabled")]
    public bool? KerberosEnabled { get; set; }

    [CommandSwitch("--kerberos5-r")]
    public string? Kerberos5R { get; set; }

    [CommandSwitch("--kerberos5-rw")]
    public string? Kerberos5Rw { get; set; }

    [CommandSwitch("--kerberos5i-r")]
    public string? Kerberos5iR { get; set; }

    [CommandSwitch("--kerberos5i-rw")]
    public string? Kerberos5iRw { get; set; }

    [CommandSwitch("--kerberos5p-r")]
    public string? Kerberos5pR { get; set; }

    [CommandSwitch("--kerberos5p-rw")]
    public string? Kerberos5pRw { get; set; }

    [CommandSwitch("--kv-private-endpoint-id")]
    public string? KvPrivateEndpointId { get; set; }

    [BooleanCommandSwitch("--ldap-enabled")]
    public bool? LdapEnabled { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--network-features")]
    public string? NetworkFeatures { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--policy-enforced")]
    public bool? PolicyEnforced { get; set; }

    [CommandSwitch("--protocol-types")]
    public string? ProtocolTypes { get; set; }

    [CommandSwitch("--remote-volume-resource-id")]
    public string? RemoteVolumeResourceId { get; set; }

    [CommandSwitch("--replication-schedule")]
    public string? ReplicationSchedule { get; set; }

    [CommandSwitch("--rule-index")]
    public string? RuleIndex { get; set; }

    [CommandSwitch("--security-style")]
    public string? SecurityStyle { get; set; }

    [CommandSwitch("--service-level")]
    public string? ServiceLevel { get; set; }

    [CommandSwitch("--smb-access")]
    public string? SmbAccess { get; set; }

    [CommandSwitch("--smb-browsable")]
    public string? SmbBrowsable { get; set; }

    [BooleanCommandSwitch("--smb-continuously-avl")]
    public bool? SmbContinuouslyAvl { get; set; }

    [BooleanCommandSwitch("--smb-encryption")]
    public bool? SmbEncryption { get; set; }

    [BooleanCommandSwitch("--snapshot-dir-visible")]
    public bool? SnapshotDirVisible { get; set; }

    [CommandSwitch("--snapshot-id")]
    public string? SnapshotId { get; set; }

    [CommandSwitch("--snapshot-policy-id")]
    public string? SnapshotPolicyId { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--throughput-mibps")]
    public string? ThroughputMibps { get; set; }

    [CommandSwitch("--unix-permissions")]
    public string? UnixPermissions { get; set; }

    [BooleanCommandSwitch("--unix-read-only")]
    public bool? UnixReadOnly { get; set; }

    [BooleanCommandSwitch("--unix-read-write")]
    public bool? UnixReadWrite { get; set; }

    [CommandSwitch("--volume-type")]
    public string? VolumeType { get; set; }

    [CommandSwitch("--zones")]
    public string? Zones { get; set; }
}