using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hdinsight", "create")]
public record AzHdinsightCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliOption("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CliOption("--autoscale-count")]
    public int? AutoscaleCount { get; set; }

    [CliOption("--autoscale-max-count")]
    public int? AutoscaleMaxCount { get; set; }

    [CliOption("--autoscale-min-count")]
    public int? AutoscaleMinCount { get; set; }

    [CliOption("--autoscale-type")]
    public string? AutoscaleType { get; set; }

    [CliOption("--cluster-admin-account")]
    public int? ClusterAdminAccount { get; set; }

    [CliOption("--cluster-admin-password")]
    public string? ClusterAdminPassword { get; set; }

    [CliOption("--cluster-configurations")]
    public string? ClusterConfigurations { get; set; }

    [CliOption("--cluster-tier")]
    public string? ClusterTier { get; set; }

    [CliOption("--cluster-users-group-dns")]
    public string? ClusterUsersGroupDns { get; set; }

    [CliOption("--component-version")]
    public string? ComponentVersion { get; set; }

    [CliFlag("--compute-isolation")]
    public bool? ComputeIsolation { get; set; }

    [CliOption("--days")]
    public int? Days { get; set; }

    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliOption("--edgenode-size")]
    public string? EdgenodeSize { get; set; }

    [CliFlag("--enable-private-link")]
    public bool? EnablePrivateLink { get; set; }

    [CliOption("--encryption-algorithm")]
    public string? EncryptionAlgorithm { get; set; }

    [CliFlag("--encryption-at-host")]
    public bool? EncryptionAtHost { get; set; }

    [CliFlag("--encryption-in-transit")]
    public bool? EncryptionInTransit { get; set; }

    [CliOption("--encryption-key-name")]
    public string? EncryptionKeyName { get; set; }

    [CliOption("--encryption-key-version")]
    public string? EncryptionKeyVersion { get; set; }

    [CliOption("--encryption-vault-uri")]
    public string? EncryptionVaultUri { get; set; }

    [CliFlag("--esp")]
    public bool? Esp { get; set; }

    [CliOption("--headnode-size")]
    public string? HeadnodeSize { get; set; }

    [CliOption("--host-sku")]
    public string? HostSku { get; set; }

    [CliOption("--http-password")]
    public string? HttpPassword { get; set; }

    [CliOption("--http-user")]
    public string? HttpUser { get; set; }

    [CliFlag("--idbroker")]
    public bool? Idbroker { get; set; }

    [CliOption("--kafka-client-group-id")]
    public string? KafkaClientGroupId { get; set; }

    [CliOption("--kafka-client-group-name")]
    public string? KafkaClientGroupName { get; set; }

    [CliOption("--kafka-management-node-count")]
    public int? KafkaManagementNodeCount { get; set; }

    [CliOption("--kafka-management-node-size")]
    public string? KafkaManagementNodeSize { get; set; }

    [CliOption("--ldaps-urls")]
    public string? LdapsUrls { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--minimal-tls-version")]
    public string? MinimalTlsVersion { get; set; }

    [CliFlag("--no-validation-timeout")]
    public bool? NoValidationTimeout { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--private-link-config")]
    public string? PrivateLinkConfig { get; set; }

    [CliOption("--resource-provider-connection")]
    public string? ResourceProviderConnection { get; set; }

    [CliOption("--ssh-password")]
    public string? SshPassword { get; set; }

    [CliOption("--ssh-public-key")]
    public string? SshPublicKey { get; set; }

    [CliOption("--ssh-user")]
    public string? SshUser { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--storage-account-key")]
    public int? StorageAccountKey { get; set; }

    [CliOption("--storage-account-managed-identity")]
    public int? StorageAccountManagedIdentity { get; set; }

    [CliOption("--storage-container")]
    public string? StorageContainer { get; set; }

    [CliOption("--storage-filesystem")]
    public string? StorageFilesystem { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--time")]
    public string? Time { get; set; }

    [CliOption("--timezone")]
    public string? Timezone { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }

    [CliOption("--workernode-count")]
    public int? WorkernodeCount { get; set; }

    [CliOption("--workernode-data-disk-size")]
    public string? WorkernodeDataDiskSize { get; set; }

    [CliOption("--workernode-data-disk-storage-account-type")]
    public int? WorkernodeDataDiskStorageAccountType { get; set; }

    [CliOption("--workernode-data-disks-per-node")]
    public string? WorkernodeDataDisksPerNode { get; set; }

    [CliOption("--workernode-size")]
    public string? WorkernodeSize { get; set; }

    [CliOption("--zones")]
    public string? Zones { get; set; }

    [CliOption("--zookeepernode-size")]
    public string? ZookeepernodeSize { get; set; }
}