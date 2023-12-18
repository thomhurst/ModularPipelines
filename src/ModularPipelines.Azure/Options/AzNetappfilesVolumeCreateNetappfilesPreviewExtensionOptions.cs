using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "volume", "create", "(netappfiles-preview", "extension)")]
public record AzNetappfilesVolumeCreateNetappfilesPreviewExtensionOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--creation-token")] string CreationToken,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--pool-name")] string PoolName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--subnet")] string Subnet,
[property: CommandSwitch("--vnet")] string Vnet
) : AzOptions
{
    [CommandSwitch("--avs-data-store")]
    public string? AvsDataStore { get; set; }

    [BooleanCommandSwitch("--backup-enabled")]
    public bool? BackupEnabled { get; set; }

    [CommandSwitch("--backup-id")]
    public string? BackupId { get; set; }

    [CommandSwitch("--backup-policy-id")]
    public string? BackupPolicyId { get; set; }

    [CommandSwitch("--backup-vault-id")]
    public string? BackupVaultId { get; set; }

    [CommandSwitch("--capacity-pool-resource-id")]
    public string? CapacityPoolResourceId { get; set; }

    [BooleanCommandSwitch("--cool-access")]
    public bool? CoolAccess { get; set; }

    [CommandSwitch("--coolness-period")]
    public string? CoolnessPeriod { get; set; }

    [CommandSwitch("--default-group-quota")]
    public string? DefaultGroupQuota { get; set; }

    [BooleanCommandSwitch("--default-quota-enabled")]
    public bool? DefaultQuotaEnabled { get; set; }

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

    [BooleanCommandSwitch("--is-large-volume")]
    public bool? IsLargeVolume { get; set; }

    [BooleanCommandSwitch("--is-restoring")]
    public bool? IsRestoring { get; set; }

    [BooleanCommandSwitch("--kerberos-enabled")]
    public bool? KerberosEnabled { get; set; }

    [CommandSwitch("--key-vault-private-endpoint-resource-id")]
    public string? KeyVaultPrivateEndpointResourceId { get; set; }

    [BooleanCommandSwitch("--ldap-enabled")]
    public bool? LdapEnabled { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--network-features")]
    public string? NetworkFeatures { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--placement-rules")]
    public string? PlacementRules { get; set; }

    [BooleanCommandSwitch("--policy-enforced")]
    public bool? PolicyEnforced { get; set; }

    [CommandSwitch("--ppg")]
    public string? Ppg { get; set; }

    [CommandSwitch("--protocol-types")]
    public string? ProtocolTypes { get; set; }

    [BooleanCommandSwitch("--relocation-requested")]
    public bool? RelocationRequested { get; set; }

    [CommandSwitch("--remote-volume-id")]
    public string? RemoteVolumeId { get; set; }

    [CommandSwitch("--remote-volume-region")]
    public string? RemoteVolumeRegion { get; set; }

    [CommandSwitch("--replication-id")]
    public string? ReplicationId { get; set; }

    [CommandSwitch("--replication-schedule")]
    public string? ReplicationSchedule { get; set; }

    [CommandSwitch("--rules")]
    public string? Rules { get; set; }

    [CommandSwitch("--security-style")]
    public string? SecurityStyle { get; set; }

    [CommandSwitch("--service-level")]
    public string? ServiceLevel { get; set; }

    [CommandSwitch("--smb-access-based-enumeration")]
    public string? SmbAccessBasedEnumeration { get; set; }

    [BooleanCommandSwitch("--smb-ca")]
    public bool? SmbCa { get; set; }

    [BooleanCommandSwitch("--smb-encryption")]
    public bool? SmbEncryption { get; set; }

    [CommandSwitch("--smb-non-browsable")]
    public string? SmbNonBrowsable { get; set; }

    [BooleanCommandSwitch("--snapshot-dir-visible")]
    public bool? SnapshotDirVisible { get; set; }

    [CommandSwitch("--snapshot-id")]
    public string? SnapshotId { get; set; }

    [CommandSwitch("--snapshot-policy-id")]
    public string? SnapshotPolicyId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--throughput-mibps")]
    public string? ThroughputMibps { get; set; }

    [CommandSwitch("--unix-permissions")]
    public string? UnixPermissions { get; set; }

    [CommandSwitch("--usage-threshold")]
    public string? UsageThreshold { get; set; }

    [CommandSwitch("--volume-spec-name")]
    public string? VolumeSpecName { get; set; }

    [CommandSwitch("--volume-type")]
    public string? VolumeType { get; set; }

    [CommandSwitch("--zones")]
    public string? Zones { get; set; }
}