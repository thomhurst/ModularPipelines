using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "azure", "clusters", "create")]
public record GcloudContainerAzureClustersCreateOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Location,
[property: CliOption("--azure-region")] string AzureRegion,
[property: CliOption("--cluster-version")] string ClusterVersion,
[property: CliOption("--fleet-project")] string FleetProject,
[property: CliOption("--pod-address-cidr-blocks")] string PodAddressCidrBlocks,
[property: CliOption("--resource-group-id")] string ResourceGroupId,
[property: CliOption("--service-address-cidr-blocks")] string ServiceAddressCidrBlocks,
[property: CliOption("--ssh-public-key")] string SshPublicKey,
[property: CliOption("--vnet-id")] string VnetId,
[property: CliOption("--client")] string Client,
[property: CliOption("--azure-application-id")] string AzureApplicationId,
[property: CliOption("--azure-tenant-id")] string AzureTenantId
) : GcloudOptions
{
    [CliOption("--admin-groups")]
    public string[]? AdminGroups { get; set; }

    [CliOption("--admin-users")]
    public string[]? AdminUsers { get; set; }

    [CliOption("--annotations")]
    public string[]? Annotations { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--config-encryption-key-id")]
    public string? ConfigEncryptionKeyId { get; set; }

    [CliOption("--config-encryption-public-key")]
    public string? ConfigEncryptionPublicKey { get; set; }

    [CliOption("--database-encryption-key-id")]
    public string? DatabaseEncryptionKeyId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable-managed-prometheus")]
    public bool? EnableManagedPrometheus { get; set; }

    [CliOption("--endpoint-subnet-id")]
    public string? EndpointSubnetId { get; set; }

    [CliOption("--logging")]
    public string[]? Logging { get; set; }

    [CliOption("--main-volume-size")]
    public string? MainVolumeSize { get; set; }

    [CliOption("--replica-placements")]
    public string[]? ReplicaPlacements { get; set; }

    [CliOption("--root-volume-size")]
    public string? RootVolumeSize { get; set; }

    [CliOption("--service-load-balancer-subnet-id")]
    public string? ServiceLoadBalancerSubnetId { get; set; }

    [CliOption("--subnet-id")]
    public string? SubnetId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--vm-size")]
    public string? VmSize { get; set; }

    [CliOption("--proxy-resource-group-id")]
    public string? ProxyResourceGroupId { get; set; }

    [CliOption("--proxy-secret-id")]
    public string? ProxySecretId { get; set; }
}