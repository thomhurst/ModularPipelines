using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "azure", "clusters", "create")]
public record GcloudContainerAzureClustersCreateOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--azure-region")] string AzureRegion,
[property: CommandSwitch("--cluster-version")] string ClusterVersion,
[property: CommandSwitch("--fleet-project")] string FleetProject,
[property: CommandSwitch("--pod-address-cidr-blocks")] string PodAddressCidrBlocks,
[property: CommandSwitch("--resource-group-id")] string ResourceGroupId,
[property: CommandSwitch("--service-address-cidr-blocks")] string ServiceAddressCidrBlocks,
[property: CommandSwitch("--ssh-public-key")] string SshPublicKey,
[property: CommandSwitch("--vnet-id")] string VnetId,
[property: CommandSwitch("--client")] string Client,
[property: CommandSwitch("--azure-application-id")] string AzureApplicationId,
[property: CommandSwitch("--azure-tenant-id")] string AzureTenantId
) : GcloudOptions
{
    [CommandSwitch("--admin-groups")]
    public string[]? AdminGroups { get; set; }

    [CommandSwitch("--admin-users")]
    public string[]? AdminUsers { get; set; }

    [CommandSwitch("--annotations")]
    public string[]? Annotations { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--config-encryption-key-id")]
    public string? ConfigEncryptionKeyId { get; set; }

    [CommandSwitch("--config-encryption-public-key")]
    public string? ConfigEncryptionPublicKey { get; set; }

    [CommandSwitch("--database-encryption-key-id")]
    public string? DatabaseEncryptionKeyId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enable-managed-prometheus")]
    public bool? EnableManagedPrometheus { get; set; }

    [CommandSwitch("--endpoint-subnet-id")]
    public string? EndpointSubnetId { get; set; }

    [CommandSwitch("--logging")]
    public string[]? Logging { get; set; }

    [CommandSwitch("--main-volume-size")]
    public string? MainVolumeSize { get; set; }

    [CommandSwitch("--replica-placements")]
    public string[]? ReplicaPlacements { get; set; }

    [CommandSwitch("--root-volume-size")]
    public string? RootVolumeSize { get; set; }

    [CommandSwitch("--service-load-balancer-subnet-id")]
    public string? ServiceLoadBalancerSubnetId { get; set; }

    [CommandSwitch("--subnet-id")]
    public string? SubnetId { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--vm-size")]
    public string? VmSize { get; set; }

    [CommandSwitch("--proxy-resource-group-id")]
    public string? ProxyResourceGroupId { get; set; }

    [CommandSwitch("--proxy-secret-id")]
    public string? ProxySecretId { get; set; }
}