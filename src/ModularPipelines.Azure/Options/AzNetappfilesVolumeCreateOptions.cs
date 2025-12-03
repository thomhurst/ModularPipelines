using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netappfiles", "volume", "create")]
public record AzNetappfilesVolumeCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--file-path")] string FilePath,
[property: CliOption("--name")] string Name,
[property: CliOption("--pool-name")] string PoolName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--usage-threshold")] string UsageThreshold,
[property: CliOption("--vnet")] string Vnet
) : AzOptions
{
    [CliFlag("--allowed-clients")]
    public bool? AllowedClients { get; set; }

    [CliOption("--avs-data-store")]
    public string? AvsDataStore { get; set; }

    [CliFlag("--backup-enabled")]
    public bool? BackupEnabled { get; set; }

    [CliOption("--backup-id")]
    public string? BackupId { get; set; }

    [CliOption("--backup-policy-id")]
    public string? BackupPolicyId { get; set; }

    [CliOption("--chown-mode")]
    public string? ChownMode { get; set; }

    [CliFlag("--cifs")]
    public bool? Cifs { get; set; }

    [CliFlag("--cool-access")]
    public bool? CoolAccess { get; set; }

    [CliOption("--coolness-period")]
    public string? CoolnessPeriod { get; set; }

    [CliOption("--default-group-quota")]
    public string? DefaultGroupQuota { get; set; }

    [CliOption("--default-user-quota")]
    public string? DefaultUserQuota { get; set; }

    [CliFlag("--delete-base-snapshot")]
    public bool? DeleteBaseSnapshot { get; set; }

    [CliFlag("--enable-subvolumes")]
    public bool? EnableSubvolumes { get; set; }

    [CliOption("--encryption-key-source")]
    public string? EncryptionKeySource { get; set; }

    [CliOption("--endpoint-type")]
    public string? EndpointType { get; set; }

    [CliFlag("--has-root-access")]
    public bool? HasRootAccess { get; set; }

    [CliFlag("--is-def-quota-enabled")]
    public bool? IsDefQuotaEnabled { get; set; }

    [CliFlag("--is-large-volume")]
    public bool? IsLargeVolume { get; set; }

    [CliFlag("--kerberos-enabled")]
    public bool? KerberosEnabled { get; set; }

    [CliOption("--kerberos5-r")]
    public string? Kerberos5R { get; set; }

    [CliOption("--kerberos5-rw")]
    public string? Kerberos5Rw { get; set; }

    [CliOption("--kerberos5i-r")]
    public string? Kerberos5iR { get; set; }

    [CliOption("--kerberos5i-rw")]
    public string? Kerberos5iRw { get; set; }

    [CliOption("--kerberos5p-r")]
    public string? Kerberos5pR { get; set; }

    [CliOption("--kerberos5p-rw")]
    public string? Kerberos5pRw { get; set; }

    [CliOption("--kv-private-endpoint-id")]
    public string? KvPrivateEndpointId { get; set; }

    [CliFlag("--ldap-enabled")]
    public bool? LdapEnabled { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--network-features")]
    public string? NetworkFeatures { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--policy-enforced")]
    public bool? PolicyEnforced { get; set; }

    [CliOption("--protocol-types")]
    public string? ProtocolTypes { get; set; }

    [CliOption("--remote-volume-resource-id")]
    public string? RemoteVolumeResourceId { get; set; }

    [CliOption("--replication-schedule")]
    public string? ReplicationSchedule { get; set; }

    [CliOption("--rule-index")]
    public string? RuleIndex { get; set; }

    [CliOption("--security-style")]
    public string? SecurityStyle { get; set; }

    [CliOption("--service-level")]
    public string? ServiceLevel { get; set; }

    [CliOption("--smb-access")]
    public string? SmbAccess { get; set; }

    [CliOption("--smb-browsable")]
    public string? SmbBrowsable { get; set; }

    [CliFlag("--smb-continuously-avl")]
    public bool? SmbContinuouslyAvl { get; set; }

    [CliFlag("--smb-encryption")]
    public bool? SmbEncryption { get; set; }

    [CliFlag("--snapshot-dir-visible")]
    public bool? SnapshotDirVisible { get; set; }

    [CliOption("--snapshot-id")]
    public string? SnapshotId { get; set; }

    [CliOption("--snapshot-policy-id")]
    public string? SnapshotPolicyId { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--throughput-mibps")]
    public string? ThroughputMibps { get; set; }

    [CliOption("--unix-permissions")]
    public string? UnixPermissions { get; set; }

    [CliFlag("--unix-read-only")]
    public bool? UnixReadOnly { get; set; }

    [CliFlag("--unix-read-write")]
    public bool? UnixReadWrite { get; set; }

    [CliOption("--volume-type")]
    public string? VolumeType { get; set; }

    [CliOption("--zones")]
    public string? Zones { get; set; }
}