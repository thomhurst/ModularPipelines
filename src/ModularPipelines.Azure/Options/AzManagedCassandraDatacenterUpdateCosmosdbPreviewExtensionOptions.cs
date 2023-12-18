using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managed-cassandra", "datacenter", "update", "(cosmosdb-preview", "extension)")]
public record AzManagedCassandraDatacenterUpdateCosmosdbPreviewExtensionOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--data-center-name")] string DataCenterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--backup-storage-customer-key-uri")]
    public string? BackupStorageCustomerKeyUri { get; set; }

    [CommandSwitch("--base64-encoded-cassandra-yaml-fragment")]
    public string? Base64EncodedCassandraYamlFragment { get; set; }

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

    [CommandSwitch("--node-count")]
    public int? NodeCount { get; set; }
}