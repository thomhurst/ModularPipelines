using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "mi", "update")]
public record AzSqlMiUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--admin-password")]
    public string? AdminPassword { get; set; }

    [CliFlag("--assign-identity")]
    public bool? AssignIdentity { get; set; }

    [CliOption("--backup-storage-redundancy")]
    public string? BackupStorageRedundancy { get; set; }

    [CliOption("--capacity")]
    public string? Capacity { get; set; }

    [CliOption("--edition")]
    public string? Edition { get; set; }

    [CliOption("--family")]
    public string? Family { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--identity-type")]
    public string? IdentityType { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--instance-pool-name")]
    public string? InstancePoolName { get; set; }

    [CliOption("--key-id")]
    public string? KeyId { get; set; }

    [CliOption("--license-type")]
    public string? LicenseType { get; set; }

    [CliOption("--maint-config-id")]
    public string? MaintConfigId { get; set; }

    [CliOption("--minimal-tls-version")]
    public string? MinimalTlsVersion { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--pid")]
    public string? Pid { get; set; }

    [CliOption("--proxy-override")]
    public string? ProxyOverride { get; set; }

    [CliFlag("--public-data-endpoint-enabled")]
    public bool? PublicDataEndpointEnabled { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--service-principal-type")]
    public string? ServicePrincipalType { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--storage")]
    public string? Storage { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--user-assigned-identity-id")]
    public string? UserAssignedIdentityId { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }

    [CliFlag("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}