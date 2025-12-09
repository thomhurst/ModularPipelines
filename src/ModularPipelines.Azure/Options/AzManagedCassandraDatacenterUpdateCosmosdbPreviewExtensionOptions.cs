using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("managed-cassandra", "datacenter", "update", "(cosmosdb-preview", "extension)")]
public record AzManagedCassandraDatacenterUpdateCosmosdbPreviewExtensionOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--data-center-name")] string DataCenterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--backup-storage-customer-key-uri")]
    public string? BackupStorageCustomerKeyUri { get; set; }

    [CliOption("--base64-encoded-cassandra-yaml-fragment")]
    public string? Base64EncodedCassandraYamlFragment { get; set; }

    [CliOption("--ldap-search-base-dn")]
    public string? LdapSearchBaseDn { get; set; }

    [CliOption("--ldap-search-filter")]
    public string? LdapSearchFilter { get; set; }

    [CliOption("--ldap-server-certs")]
    public string? LdapServerCerts { get; set; }

    [CliOption("--ldap-server-hostname")]
    public string? LdapServerHostname { get; set; }

    [CliOption("--ldap-server-port")]
    public string? LdapServerPort { get; set; }

    [CliOption("--ldap-service-user-dn")]
    public string? LdapServiceUserDn { get; set; }

    [CliOption("--ldap-svc-user-pwd")]
    public string? LdapSvcUserPwd { get; set; }

    [CliOption("--managed-disk-customer-key-uri")]
    public string? ManagedDiskCustomerKeyUri { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--node-count")]
    public int? NodeCount { get; set; }
}