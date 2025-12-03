using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managed-cassandra", "datacenter", "update")]
public record AzManagedCassandraDatacenterUpdateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--data-center-name")] string DataCenterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--backup-storage-customer-key-uri")]
    public string? BackupStorageCustomerKeyUri { get; set; }

    [CliOption("--base64-encoded-cassandra-yaml-fragment")]
    public string? Base64EncodedCassandraYamlFragment { get; set; }

    [CliOption("--managed-disk-customer-key-uri")]
    public string? ManagedDiskCustomerKeyUri { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--node-count")]
    public int? NodeCount { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }
}