using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hdinsight", "create")]
public record AzHdinsightCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--type")] string Type
) : AzOptions
{
    [CommandSwitch("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CommandSwitch("--autoscale-count")]
    public int? AutoscaleCount { get; set; }

    [CommandSwitch("--autoscale-max-count")]
    public int? AutoscaleMaxCount { get; set; }

    [CommandSwitch("--autoscale-min-count")]
    public int? AutoscaleMinCount { get; set; }

    [CommandSwitch("--autoscale-type")]
    public string? AutoscaleType { get; set; }

    [CommandSwitch("--cluster-admin-account")]
    public int? ClusterAdminAccount { get; set; }

    [CommandSwitch("--cluster-admin-password")]
    public string? ClusterAdminPassword { get; set; }

    [CommandSwitch("--cluster-configurations")]
    public string? ClusterConfigurations { get; set; }

    [CommandSwitch("--cluster-tier")]
    public string? ClusterTier { get; set; }

    [CommandSwitch("--cluster-users-group-dns")]
    public string? ClusterUsersGroupDns { get; set; }

    [CommandSwitch("--component-version")]
    public string? ComponentVersion { get; set; }

    [BooleanCommandSwitch("--compute-isolation")]
    public bool? ComputeIsolation { get; set; }

    [CommandSwitch("--days")]
    public int? Days { get; set; }

    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [CommandSwitch("--edgenode-size")]
    public string? EdgenodeSize { get; set; }

    [BooleanCommandSwitch("--enable-private-link")]
    public bool? EnablePrivateLink { get; set; }

    [CommandSwitch("--encryption-algorithm")]
    public string? EncryptionAlgorithm { get; set; }

    [BooleanCommandSwitch("--encryption-at-host")]
    public bool? EncryptionAtHost { get; set; }

    [BooleanCommandSwitch("--encryption-in-transit")]
    public bool? EncryptionInTransit { get; set; }

    [CommandSwitch("--encryption-key-name")]
    public string? EncryptionKeyName { get; set; }

    [CommandSwitch("--encryption-key-version")]
    public string? EncryptionKeyVersion { get; set; }

    [CommandSwitch("--encryption-vault-uri")]
    public string? EncryptionVaultUri { get; set; }

    [BooleanCommandSwitch("--esp")]
    public bool? Esp { get; set; }

    [CommandSwitch("--headnode-size")]
    public string? HeadnodeSize { get; set; }

    [CommandSwitch("--host-sku")]
    public string? HostSku { get; set; }

    [CommandSwitch("--http-password")]
    public string? HttpPassword { get; set; }

    [CommandSwitch("--http-user")]
    public string? HttpUser { get; set; }

    [BooleanCommandSwitch("--idbroker")]
    public bool? Idbroker { get; set; }

    [CommandSwitch("--kafka-client-group-id")]
    public string? KafkaClientGroupId { get; set; }

    [CommandSwitch("--kafka-client-group-name")]
    public string? KafkaClientGroupName { get; set; }

    [CommandSwitch("--kafka-management-node-count")]
    public int? KafkaManagementNodeCount { get; set; }

    [CommandSwitch("--kafka-management-node-size")]
    public string? KafkaManagementNodeSize { get; set; }

    [CommandSwitch("--ldaps-urls")]
    public string? LdapsUrls { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--minimal-tls-version")]
    public string? MinimalTlsVersion { get; set; }

    [BooleanCommandSwitch("--no-validation-timeout")]
    public bool? NoValidationTimeout { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--private-link-config")]
    public string? PrivateLinkConfig { get; set; }

    [CommandSwitch("--resource-provider-connection")]
    public string? ResourceProviderConnection { get; set; }

    [CommandSwitch("--ssh-password")]
    public string? SshPassword { get; set; }

    [CommandSwitch("--ssh-public-key")]
    public string? SshPublicKey { get; set; }

    [CommandSwitch("--ssh-user")]
    public string? SshUser { get; set; }

    [CommandSwitch("--storage-account")]
    public int? StorageAccount { get; set; }

    [CommandSwitch("--storage-account-key")]
    public int? StorageAccountKey { get; set; }

    [CommandSwitch("--storage-account-managed-identity")]
    public int? StorageAccountManagedIdentity { get; set; }

    [CommandSwitch("--storage-container")]
    public string? StorageContainer { get; set; }

    [CommandSwitch("--storage-filesystem")]
    public string? StorageFilesystem { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--time")]
    public string? Time { get; set; }

    [CommandSwitch("--timezone")]
    public string? Timezone { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }

    [CommandSwitch("--workernode-count")]
    public int? WorkernodeCount { get; set; }

    [CommandSwitch("--workernode-data-disk-size")]
    public string? WorkernodeDataDiskSize { get; set; }

    [CommandSwitch("--workernode-data-disk-storage-account-type")]
    public int? WorkernodeDataDiskStorageAccountType { get; set; }

    [CommandSwitch("--workernode-data-disks-per-node")]
    public string? WorkernodeDataDisksPerNode { get; set; }

    [CommandSwitch("--workernode-size")]
    public string? WorkernodeSize { get; set; }

    [CommandSwitch("--zones")]
    public string? Zones { get; set; }

    [CommandSwitch("--zookeepernode-size")]
    public string? ZookeepernodeSize { get; set; }
}

