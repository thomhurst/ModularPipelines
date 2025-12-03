using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managed-cassandra", "datacenter", "create")]
public record AzManagedCassandraDatacenterCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--data-center-location")] string DataCenterLocation,
[property: CliOption("--data-center-name")] string DataCenterName,
[property: CliOption("--delegated-subnet-id")] string DelegatedSubnetId,
[property: CliOption("--node-count")] int NodeCount,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--availability-zone")]
    public bool? AvailabilityZone { get; set; }

    [CliOption("--backup-storage-customer-key-uri")]
    public string? BackupStorageCustomerKeyUri { get; set; }

    [CliOption("--base64-encoded-cassandra-yaml-fragment")]
    public string? Base64EncodedCassandraYamlFragment { get; set; }

    [CliOption("--disk-capacity")]
    public string? DiskCapacity { get; set; }

    [CliOption("--disk-sku")]
    public string? DiskSku { get; set; }

    [CliOption("--managed-disk-customer-key-uri")]
    public string? ManagedDiskCustomerKeyUri { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }
}