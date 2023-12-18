using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managed-cassandra", "datacenter", "update")]
public record AzManagedCassandraDatacenterUpdateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--data-center-name")] string DataCenterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--backup-storage-customer-key-uri")]
    public string? BackupStorageCustomerKeyUri { get; set; }

    [CommandSwitch("--base64-encoded-cassandra-yaml-fragment")]
    public string? Base64EncodedCassandraYamlFragment { get; set; }

    [CommandSwitch("--managed-disk-customer-key-uri")]
    public string? ManagedDiskCustomerKeyUri { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--node-count")]
    public int? NodeCount { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }
}