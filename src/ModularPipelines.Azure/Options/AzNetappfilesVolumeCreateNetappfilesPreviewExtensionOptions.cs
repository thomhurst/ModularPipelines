using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "volume", "create", "(netappfiles-preview", "extension)")]
public record AzNetappfilesVolumeCreateNetappfilesPreviewExtensionOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--creation-token")] string CreationToken,
[property: CliOption("--name")] string Name,
[property: CliOption("--pool-name")] string PoolName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--subnet")] string Subnet,
[property: CliOption("--vnet")] string Vnet
) : AzOptions
{
    [CliOption("--avs-data-store")]
    public string? AvsDataStore { get; set; }

    [CliFlag("--backup-enabled")]
    public bool? BackupEnabled { get; set; }

    [CliOption("--backup-id")]
    public string? BackupId { get; set; }

    [CliOption("--backup-policy-id")]
    public string? BackupPolicyId { get; set; }

    [CliOption("--backup-vault-id")]
    public string? BackupVaultId { get; set; }

    [CliOption("--capacity-pool-resource-id")]
    public string? CapacityPoolResourceId { get; set; }

    [CliFlag("--cool-access")]
    public bool? CoolAccess { get; set; }

    [CliOption("--coolness-period")]
    public string? CoolnessPeriod { get; set; }

    [CliOption("--default-group-quota")]
    public string? DefaultGroupQuota { get; set; }

    [CliFlag("--default-quota-enabled")]
    public bool? DefaultQuotaEnabled { get; set; }

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

    [CliFlag("--is-large-volume")]
    public bool? IsLargeVolume { get; set; }

    [CliFlag("--is-restoring")]
    public bool? IsRestoring { get; set; }

    [CliFlag("--kerberos-enabled")]
    public bool? KerberosEnabled { get; set; }

    [CliOption("--key-vault-private-endpoint-resource-id")]
    public string? KeyVaultPrivateEndpointResourceId { get; set; }

    [CliFlag("--ldap-enabled")]
    public bool? LdapEnabled { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--network-features")]
    public string? NetworkFeatures { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--placement-rules")]
    public string? PlacementRules { get; set; }

    [CliFlag("--policy-enforced")]
    public bool? PolicyEnforced { get; set; }

    [CliOption("--ppg")]
    public string? Ppg { get; set; }

    [CliOption("--protocol-types")]
    public string? ProtocolTypes { get; set; }

    [CliFlag("--relocation-requested")]
    public bool? RelocationRequested { get; set; }

    [CliOption("--remote-volume-id")]
    public string? RemoteVolumeId { get; set; }

    [CliOption("--remote-volume-region")]
    public string? RemoteVolumeRegion { get; set; }

    [CliOption("--replication-id")]
    public string? ReplicationId { get; set; }

    [CliOption("--replication-schedule")]
    public string? ReplicationSchedule { get; set; }

    [CliOption("--rules")]
    public string? Rules { get; set; }

    [CliOption("--security-style")]
    public string? SecurityStyle { get; set; }

    [CliOption("--service-level")]
    public string? ServiceLevel { get; set; }

    [CliOption("--smb-access-based-enumeration")]
    public string? SmbAccessBasedEnumeration { get; set; }

    [CliFlag("--smb-ca")]
    public bool? SmbCa { get; set; }

    [CliFlag("--smb-encryption")]
    public bool? SmbEncryption { get; set; }

    [CliOption("--smb-non-browsable")]
    public string? SmbNonBrowsable { get; set; }

    [CliFlag("--snapshot-dir-visible")]
    public bool? SnapshotDirVisible { get; set; }

    [CliOption("--snapshot-id")]
    public string? SnapshotId { get; set; }

    [CliOption("--snapshot-policy-id")]
    public string? SnapshotPolicyId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--throughput-mibps")]
    public string? ThroughputMibps { get; set; }

    [CliOption("--unix-permissions")]
    public string? UnixPermissions { get; set; }

    [CliOption("--usage-threshold")]
    public string? UsageThreshold { get; set; }

    [CliOption("--volume-spec-name")]
    public string? VolumeSpecName { get; set; }

    [CliOption("--volume-type")]
    public string? VolumeType { get; set; }

    [CliOption("--zones")]
    public string? Zones { get; set; }
}