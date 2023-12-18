using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managed-cassandra", "datacenter", "create", "(cosmosdb-preview", "extension)")]
public record AzManagedCassandraDatacenterCreateCosmosdbPreviewExtensionOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--data-center-location")] string DataCenterLocation,
[property: CommandSwitch("--data-center-name")] string DataCenterName,
[property: CommandSwitch("--delegated-subnet-id")] string DelegatedSubnetId,
[property: CommandSwitch("--node-count")] int NodeCount,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--availability-zone")]
    public bool? AvailabilityZone { get; set; }

    [CommandSwitch("--backup-storage-customer-key-uri")]
    public string? BackupStorageCustomerKeyUri { get; set; }

    [CommandSwitch("--base64-encoded-cassandra-yaml-fragment")]
    public string? Base64EncodedCassandraYamlFragment { get; set; }

    [CommandSwitch("--disk-capacity")]
    public string? DiskCapacity { get; set; }

    [CommandSwitch("--disk-sku")]
    public string? DiskSku { get; set; }

    [CommandSwitch("--ldap-search-base-dn")]
    public string? LdapSearchBaseDn { get; set; }

    [CommandSwitch("--ldap-search-filter")]
    public string? LdapSearchFilter { get; set; }

    [CommandSwitch("--ldap-server-certs")]
    public string? LdapServerCerts { get; set; }

    [CommandSwitch("--ldap-server-hostname")]
    public string? LdapServerHostname { get; set; }

    [CommandSwitch("--ldap-server-port")]
    public string? LdapServerPort { get; set; }

    [CommandSwitch("--ldap-service-user-dn")]
    public string? LdapServiceUserDn { get; set; }

    [CommandSwitch("--ldap-svc-user-pwd")]
    public string? LdapSvcUserPwd { get; set; }

    [CommandSwitch("--managed-disk-customer-key-uri")]
    public string? ManagedDiskCustomerKeyUri { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }
}